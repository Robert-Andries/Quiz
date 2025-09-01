namespace Quiz.AspNetUI.Dto;

public class QuizSubmissionDto
{
    public int QuizId { get; set; }
    public List<bool> SelectedAnswer { get; set; } = new();
}