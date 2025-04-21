namespace MyMvcBackend.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; } // Lưu ý: Cần hash mật khẩu thực tế
    }
}
