using Quiz.Shared.Interfaces;

namespace Quiz.Shared.Classes;

public class QuizSettings : IQuizSettings
{
    public int NumberOfQuestions { get; set; }
    public int MaxNumberOfWrongAnswers { get; set; }
}