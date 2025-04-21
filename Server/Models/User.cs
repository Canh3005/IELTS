namespace MyMvcBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; } // Lưu ý: Cần hash mật khẩu thực tế

        public ICollection<UserTestResults> UserTestResults { get; set; } = new List<UserTestResults>();
    }
}
