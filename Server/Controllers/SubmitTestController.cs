using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;


namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/submit-test")]
    public class SubmitTestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubmitTestController(AppDbContext context)
        {
            _context = context;
            }
        

        // POST: api/submit-test
        // Nhận payload từ frontend và lưu vào cơ sở dữ liệu
        [HttpPost]
        public IActionResult SubmitTest([FromBody] UserTestResultDto dto)
        {
            // Tạo đối tượng UserTestResults từ payload
            var testResult = new UserTestResults
            {
                UserId = dto.UserId,
                TestId = dto.TestId,
                Accuracy = (float)Math.Round(dto.Accuracy, 2), // Làm tròn 2 chữ số thập phân
                Score = (float)Math.Round(dto.Score, 2), // Làm tròn 2 chữ số thập phân
                TestType = dto.TestType,
                TimeTaken = dto.TimeTaken,
                TestDate = DateTime.Now,

            };

            // Lưu vào cơ sở dữ liệu
            _context.UserTestResults.Add(testResult);
            _context.SaveChanges();
            Console.WriteLine($"Number of answers received: {dto.Answers?.Count ?? 0}");

            if (dto.Answers != null)
            {
                foreach (var answer in dto.Answers)
            {
                var userAnswer = new UserAnswer
                {
                    TestResultId = testResult.Id, // Liên kết với kết quả bài thi
                    QuestionId = answer.QuestionId,
                    UAnswer = answer.UserAnswer,
                    CorrectAnswer = answer.CorrectAnswer
                };

                _context.UserAnswers.Add(userAnswer);
                _context.SaveChanges();
                
                }
                
            }
            return Ok(new { Message = "Test result submitted successfully.", TestResultId = testResult.Id });
        }
    }

// DTO để nhận payload từ frontend
public class UserTestResultDto
{
    public int UserId { get; set; }
    public int TestId { get; set; }
    public float Accuracy { get; set; }
    public float Score { get; set; }
    public string TestType { get; set; } = string.Empty;
    public string TimeTaken { get; set; } = string.Empty;
    public List<UserAnswerDto> Answers { get; set; } = new List<UserAnswerDto>();
}

// DTO cho UserAnswer
public class UserAnswerDto
{
    public int QuestionId { get; set; }
    public string UserAnswer { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

    }

