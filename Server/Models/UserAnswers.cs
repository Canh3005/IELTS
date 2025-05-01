namespace MyMvcBackend.Models
{

public class UserAnswer
{
    public int Id { get; set; }
    public int TestResultId { get; set; } // Khóa ngoại đến bảng kết quả bài thi
    public int? QuestionId { get; set; }

    public string? TaskId {get; set;} = string.Empty; 
    public string UAnswer { get; set; } = string.Empty;
    public string? CorrectAnswer { get; set; } = string.Empty;

    public UserTestResults UserTestResult { get; set; } = null!; // Liên kết với bảng kết quả bài thi
}
}