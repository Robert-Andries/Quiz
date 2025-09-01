using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.WPFUI.Models;
using Quiz.WPFUI.Startup;
using Quiz.WPFUI.ViewModels;

namespace Quiz.WPFUI;
public partial class App
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection service = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<App>()
            .Build();

        service.AddQuizDependencyInjection();
        service.AddQuizHttpClients(configuration);
        service.AddQuizViewModels();

        service.AddSingleton<MainWindow>();
        
        service.AddScoped<Func<Type, BaseViewModel>>(_ => GetViewModel);
        
        _serviceProvider = service.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
        base.OnStartup(e);
        mainWindow.Show();
    }

    private BaseViewModel GetViewModel(Type viewModelType)
    {
        return (BaseViewModel)_serviceProvider.GetRequiredService(viewModelType);
    }
}