using Microsoft.AspNetCore.Mvc;
using Quiz.AspNetUI.Models;
using Quiz.AspNetUI.ViewModels;

namespace Quiz.AspNetUI.Controllers;

public class HomeController : Controller
{
    private readonly HealthModel _health;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, HealthModel health)
    {
        _logger = logger;
        _health = health;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GoToQuiz()
    {
        if (_health.IsUrlOk == false || _health.IsApiHealthy == false)
            return RedirectToAction("Error", "Home",
                new { message = "An internal error has occured! Please try again later." });
        return RedirectToAction("Index", "Quiz");
    }

    [HttpGet]
    public IActionResult Error(string message)
    {
        var errorViewModel = new ErrorViewModel
        {
            ErrorMessage = message
        };
        return View(errorViewModel);
    }
}