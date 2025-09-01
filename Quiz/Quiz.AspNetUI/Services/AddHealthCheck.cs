using Quiz.AspNetUI.Models;

namespace Quiz.AspNetUI.Services;

internal static class AddHealthCheck
{
    /// <summary>
    /// Adds health check service to the DI container.
    /// </summary>
    internal static async Task AddSelfHealthCheck(this WebApplicationBuilder builder)
    {
        var apiBase = builder.Configuration["Api:BaseUrl"];
        var apiQuestions = builder.Configuration["Api:QuestionsEndpoint"];
        var apiHealth = builder.Configuration["Api:HealthEndpoint"];

        builder.Services.AddSingleton(await HealthModel.CreateAsync(apiBase, apiQuestions, apiHealth));
    }
}