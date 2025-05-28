using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;
namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor
        public ProgressController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/progress/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProgress(int userId)
        {
            var results = await _context.UserTestResults
                .Where(r => r.UserId == userId)
                .ToListAsync();

            if (!results.Any())
                return Ok(new { message = "No results found." });

            // Tính tổng hợp
            var avgScore = results.Average(r => r.Score);
            var avgAccuracy = results.Where(r => r.Accuracy.HasValue).Any() ? results.Where(r => r.Accuracy.HasValue).Average(r => r.Accuracy.Value) : 0;
            // Chuyển TimeTaken (string) về giây để tính trung bình
            double AvgTimeInSeconds = 0;
            int countTime = 0;
            foreach (var r in results)
            {
                if (!string.IsNullOrWhiteSpace(r.TimeTaken) &&
                    r.TimeTaken != "NaN:NaN" &&
                    TimeSpan.TryParse(r.TimeTaken, out var ts))
                {
                    // Lọc các giá trị hợp lệ (ví dụ: 10s < time < 1h)
                    if (ts.TotalSeconds >= 10 && ts.TotalSeconds <= 3600)
                    {
                        AvgTimeInSeconds += ts.TotalSeconds;
                        countTime++;
                    }
                }
            }
            AvgTimeInSeconds = countTime > 0 ? AvgTimeInSeconds / countTime : 0;

            // Làm tròn averageScore về các mức 0.5 (IELTS)
            double roundedAvgScore = Math.Round(avgScore * 2, MidpointRounding.AwayFromZero) / 2.0;

            // Làm tròn averageAccuracy thành số nguyên gần nhất
            int roundedAvgAccuracy = (int)Math.Round(avgAccuracy, MidpointRounding.AwayFromZero);

            // Tổng số test dựa trên TestId duy nhất
            var totalTests = results.Select(r => r.TestId).Distinct().Count();

            return Ok(new
            {
                averageScore = roundedAvgScore.ToString("0.0"), // Làm tròn đến 1 chữ số thập phân
                averageAccuracy = roundedAvgAccuracy,
                averageTime = TimeSpan.FromSeconds(AvgTimeInSeconds).ToString(@"hh\:mm\:ss"),
                totalTests = totalTests,
                details = results.Select(r => new
                {
                    r.Id,
                    r.Accuracy,
                    r.Score,
                    r.TestDate,
                    r.TestType,
                    r.TimeTaken,
                    r.TestId
                }).OrderByDescending(r => r.TestDate)
            });
        }

        // GET: api/progress/skill/{userId}
        [HttpGet("skill/{userId}")]
public async Task<IActionResult> GetUserSkillProgress(int userId)
{
    var results = await _context.UserTestResults
        .Where(r => r.UserId == userId)
        .ToListAsync();

    if (!results.Any())
        return Ok(new { message = "No results found." });

    // Lấy tổng số test của từng skill từ database
    var totalListening = await _context.ListeningTests.Where(t => t.Type == "Listening").CountAsync();
    var totalReading = await _context.ListeningTests.Where(t => t.Type == "Reading").CountAsync();
    var totalSpeaking = await _context.ListeningTests.Where(t => t.Type == "Speaking").CountAsync();
    var totalWriting = await _context.ListeningTests.Where(t => t.Type == "Writing").CountAsync();

    var skillStats = results
        .GroupBy(r => r.TestType)
        .Select(g =>
        {
            var avgScore = g.Average(r => r.Score);
            var avgAccuracy = g.Where(r => r.Accuracy.HasValue).Any() ? g.Where(r => r.Accuracy.HasValue).Average(r => r.Accuracy.Value) : 0;
            double totalSeconds = 0;
            int count = 0;
            foreach (var r in g)
            {
                if (!string.IsNullOrWhiteSpace(r.TimeTaken) &&
                    r.TimeTaken != "NaN:NaN" &&
                    TimeSpan.TryParse(r.TimeTaken, out var ts) &&
                    ts.TotalSeconds >= 10 && ts.TotalSeconds <= 3600)
                {
                    totalSeconds += ts.TotalSeconds;
                    count++;
                }
            }
            double avgTime = count > 0 ? totalSeconds / count : 0;

            // Xác định tổng số test của từng skill
            int totalTest = 0;
            switch (g.Key)
            {
                case "Listening":
                    totalTest = totalListening;
                    break;
                case "Reading":
                    totalTest = totalReading;
                    break;
                case "Speaking":
                    totalTest = totalSpeaking;
                    break;
                case "Writing":
                    totalTest = totalWriting;
                    break;
            }

            return new
            {
                testType = g.Key,
                averageScore = (Math.Round(avgScore * 2, MidpointRounding.AwayFromZero) / 2.0).ToString("0.0"),
                averageAccuracy = (int)Math.Round(avgAccuracy, MidpointRounding.AwayFromZero),
                averageTime = TimeSpan.FromSeconds(avgTime).ToString(@"hh\:mm\:ss"),
                totalTestTaken = g.Select(x => x.TestId).Distinct().Count(),
                totalTest = totalTest
            };
        })
        .ToList();

    return Ok(skillStats);
}
    }
};