using System.IO;
using System.Net.Http;
using System.Windows.Input;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Value_Objects;
using Quiz.WPFUI.Dto;
using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Models;
using Quiz.WPFUI.Utilities;

namespace Quiz.WPFUI.ViewModels;

public class QuizViewModel : BaseViewModel
{
    #region Private Fields
    private string _questionNumber;
    private List<QuestionSubmissionDto> _questionsDtos;
    private string _questionText;
    #endregion
    
    public QuizViewModel(INavigationStore navigationStore, DomainLayer.Entities.Quiz quiz, Statistics statistics,
        IHttpClientFactory factory) : base(navigationStore)
    {
        NextQuestionCommand = new DelegateCommand(async void () => await NextQuestionExecute());
        Quiz = quiz;
        Statistics = statistics;
        ReloadDto();
        UpdateQuestionInfo();
        Client = factory.CreateClient("questions");
        if(Client.BaseAddress == null)
            throw new Exception("Client.BaseAddress is null");
    }

    #region Properties

    private HttpClient Client { get; set; }
    public Statistics Statistics { get; set; }

    public DomainLayer.Entities.Quiz Quiz { get; set; }

    public List<QuestionSubmissionDto> QuestionsDtos
    {
        get => _questionsDtos;
        set
        {
            if (Equals(value, _questionsDtos)) return;
            _questionsDtos = value;
            OnPropertyChanged();
        }
    }

    public string QuestionNumber
    {
        get => _questionNumber;
        set
        {
            if (value == _questionNumber) return;
            _questionNumber = value;
            OnPropertyChanged();
        }
    }

    public string QuestionText
    {
        get => _questionText;
        set
        {
            if (value == _questionText) return;
            _questionText = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand NextQuestionCommand { get; set; }

    #endregion

    #region Execute

    private async Task NextQuestionExecute()
    {
        int questionId = Quiz.CurrentQuestion!.Id;
        var selectedOptions = QuestionsDtos
            .Where(dto => dto.IsChecked)
            .Select(dto => Quiz.CurrentQuestion.Options.IndexOf(dto.OptionText))
            .Where(index => index != -1)
            .ToList();

        Quiz.Answer.Add(new SelectedAnswer
        {
            QuestionId = questionId,
            SelectedAnswers = selectedOptions
        });

        Quiz.NextQuestion();
        if (Quiz.IsComplete)
        {
            QuestionText = "Sit tight, calculating results...";
            while (await Statistics.GetStatistics(Client, Quiz) == false)
                await Task.Delay(3000);
            NavigationStore.NavigateTo<FinishViewModel>();
            return;
        }
        
        OnPropertyChanged(nameof(Quiz.CurrentQuestion.Text));
        ReloadDto();
        UpdateQuestionInfo();
    }
    #endregion

    #region Methods
    private void ReloadDto()
    {
        QuestionsDtos = Quiz.CurrentQuestion.Options
            .Distinct()
            .Select(opt => new QuestionSubmissionDto
            {
                IsChecked = false,
                OptionText = opt
            })
            .ToList();
    }
    private void UpdateQuestionInfo()
    {
        QuestionNumber = $"Question {Quiz.CurentQuestionIndex + 1} of {Quiz.Questions.Count}";
        QuestionText = Quiz.CurrentQuestion!.Text;
    }
    #endregion
}