using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/reading")]
    public class ReadingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReadingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reading
        [HttpGet]
        public async Task<IActionResult> GetListeningTests()
        {
            var readingTests = await _context.ListeningTests
            .Where(t => t.Type == "Reading") // Lọc các bài kiểm tra theo loại "reading"
            .ToListAsync();
            return Ok(readingTests);
        }

        // POST: api/reading/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateReadingTest([FromBody] ReadingTestRequest request)
        {
            // Tạo entity ReadingTest (giả sử bạn đã có model này)
            var readingTest = new ListeningTest
            {
                Title = request.Title,
                Type = "Reading",
                Duration = 40,
                NumberOfQuestions = request.Sections.Sum(s => s.Questions?.Count ?? 0) + request.Sections.Sum(s => s.BlankAnswers?.Count ?? 0),
                CreatedAt = DateTime.Now
            };
            _context.ListeningTests.Add(readingTest);
            await _context.SaveChangesAsync();

            // Lưu passages
            for (int i = 1; i <= request.Passages.Count; i++)
            {
                var passage = new ListeningRecording
                {
                    TestId = readingTest.Id,
                    Title = $"Passage {i}",
                    Passage = request.Passages[i - 1], // HTML của passage
                };
                _context.ListeningRecordings.Add(passage);
            }
            await _context.SaveChangesAsync();

            // Lưu sections
            foreach (var sectionReq in request.Sections)
            {
                var section = new ListeningPart
                {
                    RecordingId = 
                    _context.ListeningRecordings.FirstOrDefault(r => r.TestId == readingTest.Id && r.Title == $"Passage {sectionReq.PassageIndex + 1}")?.Id ?? 0,
                    Title = sectionReq.Type == "blank" ? sectionReq.Title : null, 
                    Type = sectionReq.Type == "choose" ? "Choose" : "BlankFilled",
                    ListOfQuestions = sectionReq.ListOfQuestions,
                    Text = sectionReq.TextBlankQuestion,
                };
                _context.ListeningParts.Add(section);
                await _context.SaveChangesAsync();

                // Lưu questions nếu là multi-choice
                if (sectionReq.Type == "choose" && sectionReq.Questions != null)
                {
                    foreach (var q in sectionReq.Questions)
                    {
                        var question = new ListeningQuestion
                        {
                            PartId = section.Id,
                            QuestionText = q.Question,
                        };
                        _context.ListeningQuestions.Add(question);
                        await _context.SaveChangesAsync();
                        // Lưu các options cho câu hỏi
                        foreach (var option in q.Options)
                        {
                            var questionOption = new ListeningAnswer
                            {
                                QuestionId = question.Id,
                                AnswerText = option,
                                IsCorrect = option == q.Answer // Đánh dấu đáp án đúng
                            };
                            _context.ListeningAnswers.Add(questionOption);
                        }
                    }
                    
                }

                // Lưu blank answers nếu là blank
                if (sectionReq.Type == "blank" && sectionReq.BlankAnswers != null)
                {
                    foreach (var b in sectionReq.BlankAnswers)
                    {
                        var blank = new ListeningQuestion
                        {
                            PartId = section.Id,
                            QuestionText = null, // Câu hỏi cho phần blank
                        };
                        _context.ListeningQuestions.Add(blank);
                        await _context.SaveChangesAsync();
                        var blankAnswer = new ListeningAnswer
                        {
                            QuestionId = blank.Id,
                            AnswerText = b.BlankAnswer, // Đáp án cho phần blank
                            IsCorrect = true // Giả sử đáp án này là đúng, bạn có thể điều chỉnh theo logic của bạn
                        };
                        _context.ListeningAnswers.Add(blankAnswer);
                        await _context.SaveChangesAsync();
                    }
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Reading test created successfully", success = true, id = readingTest.Id });
        }
            public class ReadingTestRequest
        {
            public string Title { get; set; }
            public List<string> Passages { get; set; } // HTML từng passage
            public List<ReadingSectionRequest> Sections { get; set; }
        }

        public class ReadingSectionRequest
        {
            public int PassageIndex { get; set; }
            public string Title { get; set; }
            public string Type { get; set; } // "choose" hoặc "blank"
            public string ListOfQuestions { get; set; } // ví dụ: "1-10"
            public List<QuestionRequest> Questions { get; set; } // cho multi-choice
            public string TextBlankQuestion { get; set; } // cho blank
            public List<BlankAnswerRequest> BlankAnswers { get; set; } // cho blank
        }

        public class QuestionRequest
        {
            public string Question { get; set; }
            public List<string> Options { get; set; }
            public string Answer { get; set; }
        }

        public class BlankAnswerRequest
        {
            public string BlankAnswer { get; set; }
        }

    }
}
