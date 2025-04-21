using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/answers")]
    public class AnswerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/parts/by-recording/5
        [HttpGet("by-question/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await _context.ListeningAnswers
                .Where(p => p.QuestionId == questionId)
                .ToListAsync();

            if (!answers.Any())
                return NotFound(new { message = "No parts found for this recording" });

            return Ok(answers);
        }
    }
}
