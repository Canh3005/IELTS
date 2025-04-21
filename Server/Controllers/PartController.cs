using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/parts/by-recording/5
        [HttpGet("by-recording/{recordingId}")]
        public async Task<IActionResult> GetPartsByRecordingId(int recordingId)
        {
            var parts = await _context.ListeningParts
                .Where(p => p.RecordingId == recordingId)
                .ToListAsync();

            if (!parts.Any())
                return NotFound(new { message = "No parts found for this recording" });

            return Ok(parts);
        }
    }
}
