using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.DomainLayer.Entities;
using Quiz.WPFUI.ViewModels;

namespace Quiz.WPFUI.Startup;

public static class AddDependencyInjection
{
    public static IServiceCollection AddQuizDependencyInjection(this IServiceCollection service)
    {
        service.AddSingleton<DomainLayer.Entities.Quiz>();
        service.AddSingleton<Statistics>();
        
        return service;
    }
}