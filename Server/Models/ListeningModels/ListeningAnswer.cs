namespace MyMvcBackend.Models
{
    public class ListeningAnswer
    {
        public int Id { get; set; }
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        //Foreign Key
        public int QuestionId { get; set; }

        public ListeningQuestion ListeningQuestion { get; set; } = null!;
        
    }
}