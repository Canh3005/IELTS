using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/results/1
        [HttpGet("{testResultId}")]
        public async Task<IActionResult> GetResultsByTestResultId(int testResultId)
        {
            var results = await _context.UserTestResults
                .FirstOrDefaultAsync(r => r.Id == testResultId);
            if (results == null)
                return NotFound(new { message = "Result not found" });

            return Ok(results);
        }

        // GET: api/results/1/answers
        [HttpGet("{testResultId}/answers")]
        public async Task<IActionResult> GetAnswersByTestResultId(int testResultId)
        {
            var answers = await _context.UserAnswers
                .Where(a => a.TestResultId == testResultId)
                .ToListAsync();
            if (answers == null || answers.Count == 0)
                return NotFound(new { message = "No answers found for this test result" });

            return Ok(answers);
        }

        // GET: api/results/test/1/user/1
        [HttpGet("test/{testId}/user/{userId}")]
        public async Task<IActionResult> GetResultsByTestIdAndUserId(int testId, int userId)
        {
            var results = await _context.UserTestResults
                .Where(r => r.TestId == testId && r.UserId == userId)
                .ToListAsync();
            if (results == null || results.Count == 0)
                return NotFound(new { message = "No results found for this test and user" });

            return Ok(results);
        } 
    }
}