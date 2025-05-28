using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/listening")]
    public class ListeningController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor
        public ListeningController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/listening
        [HttpGet]
        public async Task<IActionResult> GetListeningTests()
        {
            var listeningTests = await _context.ListeningTests
            .Where(t => t.Type == "Listening") // Lọc các bài kiểm tra theo loại "listening"
            .ToListAsync();
            return Ok(listeningTests);
        }

        // GET: api/listening/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListeningTestById(int id)
        {
            var test = await _context.ListeningTests.FirstOrDefaultAsync(t => t.Id == id);
            if (test == null)
                return NotFound(new { message = "Test not found" });

            return Ok(test); // Trả về cả test và type (của bài kiểm tra test);
        }

        // POST: api/listening/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateListeningTest([FromBody] ListeningTestRequest request)
        {
            // Tạo entity ListeningTest (giả sử bạn đã có model này)
            var listeningTest = new ListeningTest
            {
                Title = request.Title,
                Type = "Listening",
                Duration = 40,
                NumberOfQuestions = request.Sections.Sum(s => s.Questions?.Count ?? 0) + request.Sections.Sum(s => s.BlankAnswers?.Count ?? 0),
                CreatedAt = DateTime.Now
            };
            _context.ListeningTests.Add(listeningTest);
            await _context.SaveChangesAsync();

            // Lưu audios
            for (int i = 1; i <= request.Audios.Count; i++)
            {
                var audio = new ListeningRecording
                {
                    TestId = listeningTest.Id,
                    Title = $"Recording {i}",
                    AudioUrl = request.Audios[i - 1], 
                };
                _context.ListeningRecordings.Add(audio);
            }
            await _context.SaveChangesAsync();

            // Lưu sections
            foreach (var sectionReq in request.Sections)
            {
                var section = new ListeningPart
                {
                    RecordingId = 
                    _context.ListeningRecordings.FirstOrDefault(r => r.TestId == listeningTest.Id && r.Title == $"Recording {sectionReq.AudioIndex + 1}")?.Id ?? 0,
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

            return Ok(new { message = "Listening test created successfully", success = true, id = listeningTest.Id });
        }
            public class ListeningTestRequest
        {
            public string Title { get; set; }
            public List<string> Audios { get; set; } // HTML từng passage
            public List<ListeningSectionRequest> Sections { get; set; }
        }

        public class ListeningSectionRequest
        {
            public int AudioIndex { get; set; }
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
