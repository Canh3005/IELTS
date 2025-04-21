using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/recordings")]
    public class RecordingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecordingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recordings/by-test/1
        [HttpGet("by-test/{testId}")]
        public async Task<IActionResult> GetRecordingsByTestId(int testId)
        {
            var recordings = await _context.ListeningRecordings
                .Where(r => r.TestId == testId)
                .ToListAsync();

            if (!recordings.Any())
                return NotFound(new { message = "No recordings found for this test" });

            return Ok(recordings);
        }

        // GET: api/recordings/5
        [HttpGet("{recordingId}")]
public async Task<IActionResult> GetRecordingById(int recordingId)
{
    var recording = await _context.ListeningRecordings
        .Include(r => r.ListeningTest) // Bao gồm bảng ListeningTest
        .FirstOrDefaultAsync(r => r.Id == recordingId);

    if (recording == null)
        return NotFound(new { message = "Recording not found" });

    if (recording.ListeningTest == null)
        return NotFound(new { message = "Associated test not found" });

    return Ok(new
    {
        recording.Id,
        recording.Title,
        recording.AudioUrl,
        TestType = recording.ListeningTest.Type, // Trả về cột type
        NumberOfQuestions = recording.ListeningTest.NumberOfQuestions, // Trả về cột numberOfQuestions 
        recording.Passage
    });
}
    }
}
