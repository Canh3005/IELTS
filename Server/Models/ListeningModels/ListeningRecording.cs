using System.ComponentModel.DataAnnotations.Schema;
namespace MyMvcBackend.Models
{
    public class ListeningRecording
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? AudioUrl { get; set; }

        public string? Passage { get; set; } 

        // Foreign Key
        public int TestId { get; set; }

        // Navigation property for related questions
        [ForeignKey("TestId")]
        public ListeningTest ListeningTest { get; set; } = null!;
        public ICollection<ListeningPart> ListeningParts { get; set; } = new List<ListeningPart>();
}}