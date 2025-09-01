namespace Quiz.Shared.Interfaces;

public interface IQuizSettings
{
    int NumberOfQuestions { get; set; }
    int MaxNumberOfWrongAnswers { get; set; }
}