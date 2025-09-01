using Quiz.DomainLayer.Entities;
using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Models;

namespace Quiz.WPFUI.ViewModels;

public class FinishViewModel : BaseViewModel
{
    private string _finalMessage;

    public FinishViewModel(INavigationStore navigationStore, Statistics statistics) : base(navigationStore)
    {
        Statistics = statistics;
        CreateFinalMessage();
    }

    #region Properties
    public string FinalMessage
    {
        get => _finalMessage;
        set
        {
            if (value == _finalMessage) return;
            _finalMessage = value;
            OnPropertyChanged();
        }
    }
    public Statistics Statistics { get; set; }
    #endregion
    
    #region Methods
    /// <summary>
    /// Creates the final message to be displayed to the user based on their quiz performance.
    /// </summary>
    private void CreateFinalMessage()
    {
        FinalMessage = "Congratulations! You've completed the quiz.\n" +
                       $"You answered {Statistics.CorrectAnswers} out of {Statistics.NumberOfQuestions} questions correctly.\n" +
                       $"You are declared: ";
        if(Statistics.IsPassed)
            FinalMessage += "Passed!";
        else
            FinalMessage += "Not Passed!";
    }
    #endregion
}