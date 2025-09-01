using Microsoft.AspNetCore.Mvc;
using Quiz.ApplicationLayer.Services;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Quiz.Shared.Interfaces;

namespace Quiz.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class QuestionsController : Controller
{
    private readonly ILogger<QuestionsController> _logger;
    private readonly QuestionService _questionService;
    private readonly QuizService _quizService;
    private readonly IQuestionRepository _repository;
    private readonly IQuizSettings _settings;

    public QuestionsController(ILogger<QuestionsController> logger, IQuestionRepository repository,
        IQuizSettings settings)
    {
        _logger = logger;
        _repository = repository;
        _settings = settings;
        _quizService = new QuizService(_repository, _settings.MaxNumberOfWrongAnswers);
        _questionService = new QuestionService(_repository);
    }

    [HttpGet]
    public async Task<ActionResult<DomainLayer.Entities.Quiz>> GetQuestionsSet()
    {
        var allQuestions = await _repository.GetAllQuestions();
        if (allQuestions is null || !allQuestions.Any())
        {
            _logger.LogWarning("No questions found.");
            return NotFound("No questions available.");
        }

        try
        {
            var questions = _quizService.GetRandomQuestions(allQuestions, _settings.NumberOfQuestions).ToList();
            var quiz = new DomainLayer.Entities.Quiz(questions);
            await _quizService.RemoveQuizCorrectAnswers(quiz);
            return Ok(quiz);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while getting questions.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Statistics>> PostQuiz([FromBody] DomainLayer.Entities.Quiz quiz)
    {
        if (quiz is null)
        {
            _logger.LogWarning("Received null quiz.");
            return BadRequest("Quiz cannot be null.");
        }

        try
        {
            await _quizService.EnsureQuizValidity(quiz);
            var statistics = await _quizService.CalculateStatistics(quiz);
            _logger.LogInformation("Quiz processed successfully. Statistics: {Statistics}", statistics);
            return Ok(statistics);
        }
        catch (ArgumentNullException e)
        {
            _logger.LogError(e, "Quiz validation failed: {Message}", e.Message);
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Quiz processing failed: {Message}", e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred: {Message}", e.Message);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}