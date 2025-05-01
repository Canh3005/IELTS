using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Data;
using MyMvcBackend.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/writing")]
    public class WritingController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor
        public WritingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/writing
        [HttpGet]
        public async Task<IActionResult> GetWritingTests()
        {
            // Lấy các bài kiểm tra Writing từ cơ sở dữ liệu
            var writingTests = await _context.ListeningTests
                .Where(t => t.Type == "Writing") // Lọc các bài kiểm tra theo loại "Writing"
                .ToListAsync();

            // Trả về kết quả
            return Ok(writingTests);
        }

        // POST: api/writing/submit-to-feedback
        [HttpPost("submit-to-feedback")]
        public async Task<IActionResult> EvaluateWriting([FromBody] WritingRequest request)
        {
            Console.WriteLine("Đã gọi vào submit-to-feedback!");

            // Khởi tạo HttpClient
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:5173");
    client.DefaultRequestHeaders.Add("X-Title", "MyIELTSApp");

            // Xây dựng payload (body) để gửi tới API
            var deepSeekPayload = new
            {
                model = "deepseek/deepseek-r1:free", // Model cần sử dụng (có thể cần điều chỉnh nếu sử dụng model khác)
                messages = new[]
                {
                    new { role = "user", content = request.Task1Prompt }, // Bài kiểm tra task 1
                    new { role = "user", content = request.Task2Prompt }  // Bài kiểm tra task 2
                }
            };

            // Chuyển đối tượng thành chuỗi JSON và gửi yêu cầu POST đến API
            var content = new StringContent(JsonConvert.SerializeObject(deepSeekPayload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

            // Kiểm tra phản hồi từ API
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error from DeepSeek");
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
var feedbackJson = JObject.Parse(cleanedContent);

// Bước 4: Trích xuất thông tin
int task1Score = (int)feedbackJson["task1"]?["score"];
int task1TR = (int)feedbackJson["task1"]?["task response"];
int task1CC = (int)feedbackJson["task1"]?["coherence & cohesion"];
int task1LR = (int)feedbackJson["task1"]?["lexical resource"];
int task1GRA = (int)feedbackJson["task1"]?["grammatical range & accuracy"];
string task1Feedback = feedbackJson["task1"]?["feedback"]?.ToString();

int task2Score = (int)feedbackJson["task2"]?["score"];
int task2TR = (int)feedbackJson["task2"]?["task response"];
int task2CC = (int)feedbackJson["task2"]?["coherence & cohesion"];
int task2LR = (int)feedbackJson["task2"]?["lexical resource"];
int task2GRA = (int)feedbackJson["task2"]?["grammatical range & accuracy"];
string task2Feedback = feedbackJson["task2"]?["feedback"]?.ToString();

// Trả kết quả về
var result = new
{
    task1 = new { score = task1Score, tr = task1TR, cc = task1CC, lr = task1LR, gra = task1GRA, feedback = task1Feedback },
    task2 = new { score = task2Score, tr = task2TR, cc = task2CC, lr = task2LR, gra = task2GRA, feedback = task2Feedback }
};

return Ok(result);
        }
    }

    // Model nhận dữ liệu từ frontend
    public class WritingRequest
    {
        public string Task1Prompt { get; set; }
        public string Task2Prompt { get; set; }
    }
}
