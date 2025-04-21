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

    }
}
