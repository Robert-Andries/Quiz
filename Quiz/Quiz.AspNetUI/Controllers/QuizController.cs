using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.AspNetUI.Dto;
using Quiz.AspNetUI.Services;
using Quiz.AspNetUI.ViewModels;
using Quiz.DomainLayer.Value_Objects;

namespace Quiz.AspNetUI.Controllers;

public class QuizController : Controller
{
    private readonly HttpClient _client;
    private readonly ILogger<QuizController> _logger;
    private readonly QuizUiService _quizUiService;
    private QuizViewModel _viewModel;

    public QuizController(ILogger<QuizController> logger, IHttpClientFactory clientFactory, QuizUiService quizUiService)
    {
        _logger = logger;
        _quizUiService = quizUiService;
        _client = clientFactory.CreateClient("api");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            using var response = await _client.GetAsync("");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseBody))
            {
                _logger.LogWarning("Received empty response from the API.");
                return RedirectToAction("Error", "Home",
                    new { message = "An internal error has occured! Please try again later." });
            }

            DomainLayer.Entities.Quiz quiz;
            try
            {
                quiz = JsonConvert.DeserializeObject<DomainLayer.Entities.Quiz>(responseBody)
                       ?? throw new JsonSerializationException("Deserialized object is null.");
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "Failed to deserialize quiz data. Response: {ResponseBody}", responseBody);
                return RedirectToAction("Error", "Home",
                    new { message = "An internal error has occured! Please try again later." });
            }

            if (quiz.Questions.Count == 0)
            {
                _logger.LogWarning("Quiz received contains no questions.");
                return RedirectToAction("Error", "Home",
                    new { message = "An internal error has occured! Please try again later." });
            }

            quiz.CurrentQuestion!.Options = quiz.CurrentQuestion.Options.Distinct().ToList();

            await _quizUiService.SaveQuiz(quiz);
            _viewModel = new QuizViewModel(quiz);
            return View(_viewModel);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "An error occurred while fetching quiz data from the API.");
            return RedirectToAction("Error", "Home",
                new { message = "An internal error has occured! Please try again later." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Index(QuizSubmissionDto vm)
    {
        if (ModelState.IsValid == false)
        {
            _logger.LogWarning("Invalid model state received in POST request.");
            return RedirectToAction("Error", "Home",
                new { message = "An internal error has occured! Please try again later." });
        }

        DomainLayer.Entities.Quiz quiz;
        try
        {
            quiz = await _quizUiService.GetQuizById(vm.QuizId);
        }
        catch (NullReferenceException e)
        {
            _logger.LogWarning(e, "Invalid quiz ID: {QuizId}", vm.QuizId);
            return RedirectToAction("Error", "Home",
                new { message = "An internal error has occured! Please try again later." });
        }

        quiz.Answer.Add(new SelectedAnswer
        {
            QuestionId = quiz.CurrentQuestion!.Id,
            SelectedAnswers = vm.SelectedAnswer
                .Select((value, index) => new { value, index })
                .Where(x => x.value)
                .Select(x => x.index)
                .ToList()
        });
        quiz.NextQuestion();
        await _quizUiService.SaveQuiz(quiz);

        if (quiz.IsComplete)
            return RedirectToAction("Index", "Finish", new { quizId = quiz.Id });

        ModelState.Clear();
        _viewModel = new QuizViewModel(quiz);
        return View(_viewModel);
    }
}