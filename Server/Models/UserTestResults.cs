namespace MyMvcBackend.Models
{
    public class UserTestResults
    {
        public int Id { get; set; }
        public float?  Accuracy { get; set; }
        public float  Score { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;
        public string TestType { get; set; } = string.Empty;
        public string TimeTaken { get; set; } = string.Empty;
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

        // Foreign Key
        public int UserId { get; set; }
        public int TestId { get; set; }

        public User User { get; set; } = null!;
        public ListeningTest ListeningTest { get; set; } = null!;

    }
}