using Microsoft.EntityFrameworkCore;
using Quiz.DomainLayer.Interfaces;
using Quiz.PersistenceLayer;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.AspNetUI.Services;

internal static class DiContainer
{
    internal static void AddQuizDependencyInjection(this IServiceCollection services)
    {
        services.AddDbContext<QuizDbContext>(options => { options.UseInMemoryDatabase("QuizDb"); });
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<QuizUiService>();
    }
}