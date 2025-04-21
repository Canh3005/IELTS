using Microsoft.AspNetCore.Mvc;
using MyMvcBackend.Data;
using MyMvcBackend.Models;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MyMvcBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest(new { message = "This email already exists!" });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash mật khẩu
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Register successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            // Tạo token 
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                return StatusCode(500, new { message = "JWT SecretKey is not configured." });
            }
            var key = Encoding.UTF8.GetBytes(secretKey); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim(ClaimTypes.Email, existingUser.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                Issuer = "MyMvcBackend",
                Audience = "MyMvcBackend",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { message = "Login successful.",
                token = tokenString, 
                username = existingUser.Username,
                userId = existingUser.Id.ToString(),
            });
        }
    }
}
