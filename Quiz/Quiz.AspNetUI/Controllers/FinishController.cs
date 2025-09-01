using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.AspNetUI.Services;
using Quiz.DomainLayer.Entities;

namespace Quiz.AspNetUI.Controllers;

public class FinishController : Controller
{
    private readonly HttpClient _client;
    private readonly ILogger<FinishController> _logger;
    private readonly QuizUiService _quizUiService;

    public FinishController(ILogger<FinishController> logger, QuizUiService quizUiService,
        IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _quizUiService = quizUiService;
        _client = clientFactory.CreateClient("api");
    }

    [HttpGet]
    public async Task<IActionResult> Index(int quizId)
    {
        var quiz = await _quizUiService.GetQuizById(quizId);
        var jsonText = JsonConvert.SerializeObject(quiz);
        try
        {
            using var response =
                await _client.PostAsync("", new StringContent(jsonText, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<Statistics>(responseBody);
            if (statistics == null)
                return RedirectToAction("Error", "Home",
                    new { message = "An internal error has occured! Please try again later." });
            return View(statistics);
        }
        catch (Exception e)
        {
            return RedirectToAction("Error", "Home",
                new { message = "An internal error has occured! Please try again later." });
        }
    }
}