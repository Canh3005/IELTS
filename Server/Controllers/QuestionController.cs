using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/questions/by-part/5
        [HttpGet("by-part/{partId}")]
        public async Task<IActionResult> GetQuestionsByPartId(int partId)
        {
            var questions = await _context.ListeningQuestions
                .Where(p => p.PartId == partId)
                .ToListAsync();

            if (!questions.Any())
                return NotFound(new { message = "No parts found for this recording" });

            return Ok(questions);
        }
    }
}
