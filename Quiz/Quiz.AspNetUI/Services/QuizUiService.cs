using Quiz.DomainLayer.Interfaces;

namespace Quiz.AspNetUI.Services;

/// <summary>
/// A service for managing quizzes in the UI layer.
/// </summary>
public class QuizUiService
{
    private readonly IQuizRepository _quizRepository;

    public QuizUiService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    /// <summary>
    /// Gets a quiz by its ID.
    /// </summary>
    /// <param name="id">Quiz id to search for</param>
    /// <exception cref="NullReferenceException">Quiz with provided id was not found</exception>
    public async Task<DomainLayer.Entities.Quiz> GetQuizById(int id)
    {
        var output = await _quizRepository.GetQuizById(id);
        if (output == null)
            throw new NullReferenceException("Quiz not found");
        return output;
    }


    /// <summary>
    /// Saves the quiz to the database.
    /// </summary>
    /// <param name="quiz">Quiz to save</param>
    public async Task SaveQuiz(DomainLayer.Entities.Quiz quiz)
    {
        await _quizRepository.SaveQuiz(quiz);
    }
}