namespace Quiz.DomainLayer.Entities;

public class Statistics
{
    public Statistics()
    {
        
    }
    public Statistics(int numberOfQuestions, int numberOfWrongAnswers, int maxWrongAnswers)
    {
        NumberOfQuestions = numberOfQuestions;
        WrongAnswers = numberOfWrongAnswers;
        IsPassed = numberOfWrongAnswers <= maxWrongAnswers;
        CorrectAnswers = NumberOfQuestions - WrongAnswers;
    }

    public int Id { get; set; }
    public bool IsPassed { get; set; }
    public int NumberOfQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }

    public override string ToString()
    {
        return $"IsPassed: {IsPassed}, " +
               $"NumberOfQuestions: {NumberOfQuestions}, " +
               $"CorrectAnswers: {CorrectAnswers}, " +
               $"WrongAnswers: {WrongAnswers}";
    }
}