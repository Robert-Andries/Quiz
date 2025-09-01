using Quiz.DomainLayer.Interfaces;

namespace Quiz.AspNetUI.Services;

public class QuizUiService
{
    private readonly IQuizRepository _quizRepository;

    public QuizUiService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<DomainLayer.Entities.Quiz> GetQuizById(int id)
    {
        var output = await _quizRepository.GetQuizById(id);
        if (output == null)
            throw new NullReferenceException("Quiz not found");
        return output;
    }


    public async Task SaveQuiz(DomainLayer.Entities.Quiz quiz)
    {
        await _quizRepository.SaveQuiz(quiz);
    }
}