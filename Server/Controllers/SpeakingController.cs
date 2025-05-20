using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


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

        // POST: api/speaking/submit-to-feedback
        [HttpPost("submit-to-feedback")]
        public async Task<IActionResult> EvaluateSpeaking([FromBody] SpeakingRequest request)
        {
            Console.WriteLine("Đã gọi vào submit-to-feedback!");

            // Khởi tạo HttpClient
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:8080");
            client.DefaultRequestHeaders.Add("X-Title", "MyIELTSApp");

            // Xây dựng payload (body) để gửi tới API
            var deepSeekPayload = new
            {
                model = "deepseek/deepseek-r1:free", // Model cần sử dụng (có thể cần điều chỉnh nếu sử dụng model khác)
                messages = new[]
                {
                    new { role = "user", content = request.SpeakingPrompt }, // Bài kiểm tra task 1
                }
            };

            // Chuyển đối tượng thành chuỗi JSON và gửi yêu cầu POST đến API
            var content = new StringContent(JsonConvert.SerializeObject(deepSeekPayload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

            // Kiểm tra phản hồi từ API
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error Response: " + errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }

            // Đọc và trả về nội dung phản hồi từ API
            var responseBody = await response.Content.ReadAsStringAsync();
            

            // Bước 1: Parse phản hồi gốc

var json = JObject.Parse(responseBody);


var rawContent = json["choices"]?[0]?["message"]?["content"]?.ToString();

// Bước 2: Làm sạch đoạn ```json ... ```
string cleanedContent = rawContent
    .Replace("```json", "")
    .Replace("```", "")
    .Trim();

// Bước 3: Parse JSON thực sự bên trong content
// Parse JSON thực sự bên trong content
var feedbackJson = JObject.Parse(cleanedContent);
Console.WriteLine("Response Body: " + feedbackJson.ToString());

// Lấy thông tin Part 1
var part1Transcript = feedbackJson["part1"]?["transcript"]?.ToString() ?? "No answer";
var part1Feedback = feedbackJson["part1"]?["feedback"]?.ToString() ?? "No feedback";
var part1Score = feedbackJson["part1"]?["score"]?.ToObject<int>() ?? -1;
var part1FC = feedbackJson["part1"]?["fluency and coherence"]?.ToObject<int>() ?? -1;
var part1LR = feedbackJson["part1"]?["lexical resource"]?.ToObject<int>() ?? -1;
var part1GRA = feedbackJson["part1"]?["grammatical range and accuracy"]?.ToObject<int>() ?? -1;
var part1PR = feedbackJson["part1"]?["pronunciation"]?.ToObject<int>() ?? -1;

// Lấy thông tin Part 2
var part2Transcript = feedbackJson["part2"]?["transcript"]?.ToString() ?? "No answer";
var part2Feedback = feedbackJson["part2"]?["feedback"]?.ToString() ?? "No feedback";
var part2Score = feedbackJson["part2"]?["score"]?.ToObject<int>() ?? -1;
var part2FC = feedbackJson["part2"]?["fluency and coherence"]?.ToObject<int>() ?? -1;
var part2LR = feedbackJson["part2"]?["lexical resource"]?.ToObject<int>() ?? -1;
var part2GRA = feedbackJson["part2"]?["grammatical range and accuracy"]?.ToObject<int>() ?? -1;
var part2PR = feedbackJson["part2"]?["pronunciation"]?.ToObject<int>() ?? -1;

// Lấy thông tin Part 3
var part3Transcript = feedbackJson["part3"]?["transcript"]?.ToString() ?? "No answer";
var part3Feedback = feedbackJson["part3"]?["feedback"]?.ToString() ?? "No feedback";
var part3Score = feedbackJson["part3"]?["score"]?.ToObject<int>() ?? -1;
var part3FC = feedbackJson["part3"]?["fluency and coherence"]?.ToObject<int>() ?? -1;
var part3LR = feedbackJson["part3"]?["lexical resource"]?.ToObject<int>() ?? -1;
var part3GRA = feedbackJson["part3"]?["grammatical range and accuracy"]?.ToObject<int>() ?? -1;
var part3PR = feedbackJson["part3"]?["pronunciation"]?.ToObject<int>() ?? -1;

// Tạo đối tượng chứa thông tin quan trọng
var extractedData = new
{
    task1 = new
    {
        transcript = part1Transcript,
        score = part1Score,
        fc = part1FC,
        lr = part1LR,
        gra = part1GRA,
        pr = part1PR,
        feedback = part1Feedback
    },
    task2 = new
    {
        transcript = part2Transcript,
        score = part2Score,
        fc = part2FC,
        lr = part2LR,
        gra = part2GRA,
        pr = part2PR,
        feedback = part2Feedback
    },
    task3 = new
    {
        transcript = part3Transcript,
        score = part3Score,
        fc = part3FC,
        lr = part3LR,
        gra = part3GRA,
        pr = part3PR,
        feedback = part3Feedback
    }
};


var testResult = new UserTestResults
{
    UserId = request.UserId, // Thay đổi theo ID người dùng thực tế
    TestId = request.TestId, // Thay đổi theo ID bài kiểm tra thực tế
    Accuracy = 0, // Chưa có thông tin này từ API
    Score = (float)(part1Score + part2Score + part3Score) / 3, // Tính điểm trung bình của cả hai bài kiểm tra
    TestType = "Speaking",
    TimeTaken = request.TimeTaken, // Chưa có thông tin này từ API
    TestDate = DateTime.Now,
};

_context.UserTestResults.Add(testResult);
await _context.SaveChangesAsync();

// Trả về kết quả
return Ok(extractedData);
        }
    }

    // Model nhận dữ liệu từ frontend
    public class SpeakingRequest
    {
        public string SpeakingPrompt { get; set; } = string.Empty; // Đề bài Speaking
        public int UserId { get; set; } // ID người dùng (nếu cần thiết)
        public int TestId { get; set; } // ID bài kiểm tra (nếu cần thiết)
        public float Score { get; set; } // Điểm (nếu cần thiết)
        public string TimeTaken { get; set; } // Thời gian làm bài (nếu cần thiết)
        public string TestType { get; set; } // Loại bài kiểm tra (nếu cần thiết)
    }

    }

