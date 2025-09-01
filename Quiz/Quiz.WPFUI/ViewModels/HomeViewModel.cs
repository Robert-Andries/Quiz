using System.Net.Http;
using System.Windows.Input;
using Newtonsoft.Json;
using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Models;
using Quiz.WPFUI.Utilities;

namespace Quiz.WPFUI.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private string _welcomeMessage;
    private readonly HttpClient _baseClient;
    private readonly HttpClient _quizClient;
    private DomainLayer.Entities.Quiz _quiz;

    public HomeViewModel(INavigationStore navigationStore, IHttpClientFactory factory, DomainLayer.Entities.Quiz quiz) : base(navigationStore)
    {
        _quiz = quiz;
        _baseClient = factory.CreateClient("apiBaseAdress");
        _quizClient = factory.CreateClient("questions");
        WelcomeMessage = "Press the below button when you are ready. Make sure you have stabile internet, and good luck!";
        StartCommand = new DelegateCommand(async void () => await StartExecute());
    }

    #region Proprieties
    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set
        {
            _welcomeMessage = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    #region Commands
    public ICommand StartCommand { get; set; }
    #endregion
    
    #region Execute
    private async Task StartExecute()
    {
        WelcomeMessage = "We are trying to connect you to the quiz. Please wait...";
        if (await ApiUtilities.IsApiIsRunning(_baseClient) == false)
        {
            WelcomeMessage = "The API is currently unavailable. Please try again later.";
            return;
        }
        try
        {
            var response = await _quizClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            
            JsonConvert.PopulateObject(content, _quiz);
            
            if(_quiz.Questions.Count == 0)
            {
                WelcomeMessage = "The API is currently unavailable. Please try again later.";
                return;
            }
        }
        catch (Exception)
        {
            WelcomeMessage = "The API is currently unavailable. Please try again later.";
            return;
        }
        NavigationStore.NavigateTo<QuizViewModel>();
    }
    #endregion
}
