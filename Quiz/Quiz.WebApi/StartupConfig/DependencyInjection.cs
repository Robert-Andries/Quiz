using Microsoft.Extensions.Options;
using Quiz.DomainLayer.Interfaces;
using Quiz.PersistenceLayer;
using Quiz.Shared.Classes;
using Quiz.Shared.Interfaces;

namespace Quiz.WebApi.StartupConfig;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<QuizSettings>(
            builder.Configuration.GetSection("QuizSettings"));
        builder.Services.AddSingleton<IQuizSettings>(sp => sp.GetRequiredService<IOptions<QuizSettings>>().Value);
        builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
    }
}