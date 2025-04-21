namespace MyMvcBackend.Models
{
    public class ListeningPart
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public required string Text { get; set; }
        public required string Type { get; set; } 
        public string? ListOfQuestions { get; set; } // Total number of questions in the part
        public string? ImageUrl { get; set; }

        //Foreign Key
        public int RecordingId { get; set; }

        public ListeningRecording ListeningRecording { get; set; } = null!;
        
        public ICollection<ListeningQuestion> ListeningQuestions { get; set; } = new List<ListeningQuestion>();
    }
}