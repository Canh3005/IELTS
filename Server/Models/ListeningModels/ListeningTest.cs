namespace MyMvcBackend.Models
{
    public class ListeningTest
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public int Duration { get; set; } // Duration in minutes
        public int? NumberOfQuestions { get; set; } // Total number of questions in the test
        public string? Type { get; set; } // Type of the test (e.g., listening, reading, writing, speaking etc.)

        // Navigation property for related questions
        public ICollection<ListeningRecording> ListeningRecordings { get; set; } = new List<ListeningRecording>();

        public ICollection<UserTestResults> UserTestResults { get; set; } = new List<UserTestResults>();
}}