using Microsoft.Extensions.DependencyInjection;
using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Stores;
using Quiz.WPFUI.ViewModels;

namespace Quiz.WPFUI.Startup;

public static class AddViewModels
{
    public static IServiceCollection AddQuizViewModels(this IServiceCollection service)
    {
        service.AddScoped<INavigationStore, NavigationStore>();
        service.AddScoped<MainViewModel>();
        service.AddScoped<HomeViewModel>();
        service.AddScoped<QuizViewModel>();
        service.AddScoped<FinishViewModel>();
        
        return service;
    }
}