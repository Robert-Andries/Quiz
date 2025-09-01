namespace Quiz.AspNetUI.Dto;

/// <summary>
/// Class representing a quiz submission.
/// </summary>
public class QuizSubmissionDto
{
    public int QuizId { get; set; }
    public List<bool> SelectedAnswer { get; set; } = new();
}