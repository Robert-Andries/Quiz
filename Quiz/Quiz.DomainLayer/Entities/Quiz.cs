using Quiz.DomainLayer.Value_Objects;

namespace Quiz.DomainLayer.Entities;

public class Quiz
{
    public Quiz()
    {
        Questions = new List<Question>();
        IsComplete = false;
        CurentQuestionIndex = 0;
        Answer = new List<SelectedAnswer>();
    }

    public Quiz(List<Question> questions)
    {
        if (questions is null || questions.Count == 0)
            throw new ArgumentException("Questions cannot be null or empty.", nameof(questions));
        Questions = questions;
        IsComplete = false;
        CurentQuestionIndex = 0;
        Answer = new List<SelectedAnswer>();
    }

    public int Id { get; set; }
    public int CurentQuestionIndex { get; set; }
    public bool IsComplete { get; set; }
    public int NumberOfQuestions => Questions.Count();
    public int RemainingQuestions => Questions.Count() - CurentQuestionIndex - 1;
    public List<Question> Questions { get; set; }

    public Question? CurrentQuestion =>
        Questions.Count() > 0 && CurentQuestionIndex >= 0 && CurentQuestionIndex < Questions.Count()
            ? Questions.ElementAt(CurentQuestionIndex)
            : null;

    /// <summary>
    ///     Selected answers by the user for a specific question
    /// </summary>
    public List<SelectedAnswer> Answer { get; set; }


    public void NextQuestion()
    {
        if (IsComplete)
            throw new InvalidOperationException("Quiz is already complete.");

        if (CurentQuestionIndex < Questions.Count - 1)
            CurentQuestionIndex++;
        else
            IsComplete = true;
    }
}