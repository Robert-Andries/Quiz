using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Quiz.WPFUI.Startup;

public static class AddHttpClients
{
    /// <summary>
    /// Adds named HTTP clients for quiz functionality to the service collection.
    /// </summary>
    public static IServiceCollection AddQuizHttpClients(this IServiceCollection service, IConfiguration config)
    {
        service.AddHttpClient("questions", opts =>
        {
            opts.BaseAddress = new Uri(config["Api:QuestionsEndpoint"] ?? throw new NullReferenceException("QuestionsEndpoint is null"));
        });
        service.AddHttpClient("apiBaseAdress", opts =>
        {
            opts.BaseAddress = new Uri(config["Api:BaseUrl"] ?? throw new NullReferenceException("BaseUrl is null"));
        });
        
        return service;
    }
}