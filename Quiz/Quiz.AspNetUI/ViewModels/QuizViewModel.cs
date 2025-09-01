using Quiz.DomainLayer.Entities;

namespace Quiz.AspNetUI.ViewModels;

public class QuizViewModel
{
    public QuizViewModel()
    {
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

    public int QuizId { get; set; }
    public DomainLayer.Entities.Quiz QuizModel { get; set; }
    public Question CurrentQuestion { get; }
    public int RemainingQuestions { get; private set; }
    public List<bool> SelectedAnswer { get; set; }
}