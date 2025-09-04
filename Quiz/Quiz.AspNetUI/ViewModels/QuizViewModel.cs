using System.ComponentModel.DataAnnotations;
using Quiz.DomainLayer.Entities;

namespace Quiz.AspNetUI.ViewModels;

public class QuizViewModel
{
    public QuizViewModel()
    {
        QuizModel = new DomainLayer.Entities.Quiz();
        CurrentQuestion = new Question();
        SelectedAnswer = new List<bool>();
    }

    public QuizViewModel(DomainLayer.Entities.Quiz quizModel)
    {
        QuizId = quizModel.Id;
        QuizModel = quizModel;
        CurrentQuestion = quizModel.CurrentQuestion!;
        RemainingQuestions = quizModel.RemainingQuestions;
        SelectedAnswer = new List<bool>();
        for (var i = 0; i < CurrentQuestion.Options.Count; i++)
            SelectedAnswer.Add(false);
    }

    [Required]
    public int QuizId { get; set; }
    [Required]
    public DomainLayer.Entities.Quiz QuizModel { get; set; }
    [Required]
    public Question CurrentQuestion { get; }
    [Required]
    public int RemainingQuestions { get; private set; }
    public List<bool> SelectedAnswer { get; set; }
}