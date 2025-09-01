using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;

namespace Quiz.ApplicationLayer.Services;

public class QuizService
{
    private readonly int _maxNumberOfWrongAnswers;
    private readonly IQuestionRepository _questionRepository;
    private readonly QuestionService _questionService;

    public QuizService(IQuestionRepository questionRepository, int maxNumberOfWrongAnswers)
    {
        _questionRepository = questionRepository;
        _questionService = new QuestionService(_questionRepository);
        _maxNumberOfWrongAnswers = maxNumberOfWrongAnswers;
    }

    /// <summary>
    ///     Ensures that the quiz and all questions inside it are valid. Throws exception if any validation fails.
    /// </summary>
    public async Task EnsureQuizValidity(DomainLayer.Entities.Quiz quiz, bool checkRemainingQuestions = true,
        bool checkQuestions = true)
    {
        if (quiz is null)
            throw new ArgumentNullException(nameof(quiz), "Quiz cannot be null.");
        if (checkQuestions && (quiz.Questions is null || !quiz.Questions.Any()))
            throw new ArgumentException("Quiz must contain questions.", nameof(quiz));
        if (checkRemainingQuestions && quiz.RemainingQuestions > 0)
            throw new InvalidOperationException("Quiz is not complete. There are remaining questions to answer.");
        foreach (var question in quiz.Questions) await _questionService.EnsureQuestionValidity(question);
    }

    /// <summary>
    ///     Calculates and returns the statistics for a given quiz.
    /// </summary>
    /// <exception cref="InvalidOperationException">There is a question in input that is not present on server</exception>
    public async Task<Statistics> CalculateStatistics(DomainLayer.Entities.Quiz quiz)
    {
        await EnsureQuizValidity(quiz);
        var numberOfQuestions = quiz.NumberOfQuestions;
        var numberOfWrongAnswers = 0;
        foreach (var answer in quiz.Answer)
            if (await _questionService.CheckQuestionResponse(answer) == false)
                numberOfWrongAnswers++;

        return new Statistics(numberOfQuestions, numberOfWrongAnswers, _maxNumberOfWrongAnswers);
    }

    /// <summary>
    ///     Gets random elements from a list
    /// </summary>
    /// <param name="source">The source list</param>
    /// <param name="ammount">How many questions you want</param>
    /// <returns>A List of random objects with the size of ammount</returns>
    /// <exception cref="ArgumentNullException">Source is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">The ammount is either too big or smaller then 0</exception>
    public IEnumerable<Question> GetRandomQuestions(List<Question> source, int ammount)
    {
        // Check if source is null or empty
        if (source is null || source.Count == 0)
            throw new ArgumentNullException(nameof(source));

        var sourceCount = source.Count;

        // Check if its possible to have the desired ammount
        // of elements in the newly created list
        if (ammount < 0 || ammount > sourceCount) throw new ArgumentOutOfRangeException(nameof(ammount));

        var random = new Random();
        return source.OrderBy(_ => random.Next()).Take(ammount).ToList();
    }

    /// <summary>
    ///     Removes correct answers from the quiz questions.
    /// </summary>
    public async Task RemoveQuizCorrectAnswers(DomainLayer.Entities.Quiz quiz)
    {
        await EnsureQuizValidity(quiz, false);
        foreach (var question in quiz.Questions) await _questionService.RemoveQuestionCorrectAnswers(question);
    }

    /// <summary>
    ///     Adds correct answers to the quiz questions.
    /// </summary>
    public async Task AddQuizCorrectAnswers(DomainLayer.Entities.Quiz quiz)
    {
        await EnsureQuizValidity(quiz);
        foreach (var question in quiz.Questions) await _questionService.AddQuestionCorrectAnswers(question);
    }
}