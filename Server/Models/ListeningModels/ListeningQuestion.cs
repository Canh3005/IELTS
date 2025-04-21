namespace MyMvcBackend.Models
{
    public class ListeningQuestion
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }

        //Foreign Key
        public int PartId { get; set; }

        public ListeningPart ListeningPart { get; set; }= null!;

        public ICollection<ListeningAnswer> ListeningAnswers { get; set; } = new List<ListeningAnswer>();
        
    }
}