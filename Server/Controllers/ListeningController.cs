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

        public ListeningController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/listening
        [HttpGet]
        public async Task<IActionResult> GetListeningTests()
        {
            var listeningTests = await _context.ListeningTests
            .Where (t => t.Type == "Listening") // Lọc các bài kiểm tra theo loại "listening"
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
    }
}
