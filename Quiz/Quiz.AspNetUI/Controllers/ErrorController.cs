using Microsoft.AspNetCore.Mvc;

namespace Quiz.AspNetUI.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}