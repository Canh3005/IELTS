using Microsoft.AspNetCore.Mvc;
using MyMvcBackend.Data;
using Microsoft.EntityFrameworkCore;
namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/answers")]
    public class CorrectAnswerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CorrectAnswerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/answers/1
        [HttpGet("{testId}")]
        public async Task<IActionResult> GetCorrectAnswersByTestId(int testId)
        {
            var correctAnswers = await _context.ListeningAnswers
                .Where(c => c.IsCorrect && c.ListeningQuestion.ListeningPart.ListeningRecording.TestId == testId)
                .Select(a => new
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.Id,
                    AnswerText = a.AnswerText != null && a.AnswerText.Contains(".")
                    ? a.AnswerText.Substring(0,1).Trim()
                    : a.AnswerText
                })
                .ToListAsync();

            
            if (correctAnswers == null || correctAnswers.Count == 0)
                return NotFound(new { message = "No correct answers found for this test" });

            // Gộp các đáp án theo QuestionId
            var groupedAnswers = correctAnswers
            .GroupBy(a => a.QuestionId)
            .Select(group => new
            {
                QuestionId = group.Key,
                AnswerText = string.Join("/", group.Select(a => a.AnswerText).Distinct()) // Gộp các đáp án thành A/B/C
            })
            .ToList();

            var orderedAnswers = groupedAnswers
            .Select((answer, index) => new
            {
                Index = index+1,
                answer.QuestionId,
                answer.AnswerText
            })
            .ToList();

            return Ok(orderedAnswers);
        }
    }
}