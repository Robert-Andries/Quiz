using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Quiz.DomainLayer.Value_Objects;

namespace Quiz.ApplicationLayer.Services;

public class QuestionService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    /// <summary>
    ///     Ensures that the question is valid. Throws exception if any validation fails.
    /// </summary>
    /// <exception cref="ArgumentNullException">Question provided is null</exception>
    /// <exception cref="ArgumentException">Provided question id is not present on repository</exception>
    public async Task EnsureQuestionValidity(Question? question)
    {
        if (question is null)
            throw new ArgumentNullException(nameof(question));
        if (await _questionRepository.GetQuestionById(question.Id) is null)
            throw new ArgumentException($"Question with id {question.Id} is not found on repository.");
    }

    /// <summary>
    ///     Check if the provided answer matches the correct options for the question.
    /// </summary>
    /// <param name="answer">Provided answer</param>
    /// <returns>Bool indicating if the answer is corect or not</returns>
    /// <exception cref="ArgumentNullException">Answer is null</exception>
    /// <exception cref="ArgumentException">The id from input is not found in repository</exception>
    public async Task<bool> CheckQuestionResponse(SelectedAnswer answer)
    {
        if (answer is null)
            throw new ArgumentNullException(nameof(answer));
        var question = await _questionRepository.GetQuestionById(answer.QuestionId);
        if (question is null)
            throw new ArgumentException($"Question with id {answer.QuestionId} is not found on repository.");

        var correctOptions = question.CorectOption.Distinct();
        if (correctOptions.Count() != answer.SelectedAnswers.Count())
            return false;

        foreach (var selectedAnswer in answer.SelectedAnswers)
            if (correctOptions.Contains(selectedAnswer) == false)
                return false;

        return true;
    }

    /// <summary>
    ///     Removes correct answers from the question.
    /// </summary>
    public async Task RemoveQuestionCorrectAnswers(Question question)
    {
        await EnsureQuestionValidity(question);
        question.CorectOption = new List<int>();
    }

    /// <summary>
    ///     Add correct answers to the question.
    /// </summary>
    /// <param name="question"></param>
    public async Task AddQuestionCorrectAnswers(Question question)
    {
        await EnsureQuestionValidity(question);
        var questionFromRepository = await _questionRepository.GetQuestionById(question.Id);
        var correctOptions = questionFromRepository!.CorectOption.Distinct().ToList();
        question.CorectOption = correctOptions;
    }
}