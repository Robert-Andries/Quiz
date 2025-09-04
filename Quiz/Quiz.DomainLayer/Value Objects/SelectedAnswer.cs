namespace Quiz.DomainLayer.Value_Objects;

public class SelectedAnswer
{
    public SelectedAnswer()
    {
        SelectedAnswers = new List<int>();
    }

    public SelectedAnswer(int questionId, List<int> selectedAnswers)
    {
        QuestionId = questionId;
        SelectedAnswers = selectedAnswers;
    }

    public int Id { get; set; }
    public int QuestionId { get; set; }
    public List<int> SelectedAnswers { get; set; }
}