using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/speaking")]
    public class SpeakingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpeakingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/speaking
        [HttpGet]
        public async Task<IActionResult> GetListeningTests()
        {
            var speakingTests = await _context.ListeningTests
            .Where(t => t.Type == "Speaking")
            .ToListAsync();
            return Ok(speakingTests);
        }

    }
}
