namespace Quiz.DomainLayer.Entities;

public class Question
{
    public Question()
    {
        Options = new List<string>();
        CorectOption = new List<int>();
        Text  = string.Empty;
    }

    public Question(int id, string text, string? textSecond, List<string> options, List<int> corectOption,
        QuestionType type)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or whitespace.", nameof(text));
        if (options == null || options.Count == 0)
            throw new ArgumentException("Options cannot be null or empty.", nameof(options));
        if (corectOption == null || corectOption.Count < 2)
            throw new ArgumentException("Correct options cannot be null or empty.", nameof(corectOption));

        Id = id;
        Text = text;
        TextSecond = textSecond;
        Options = options;
        CorectOption = corectOption;
        Type = type;
    }

    public int Id { get; set; }
    public string Text { get; set; }
    public string? TextSecond { get; set; }
    public List<string> Options { get; set; }
    public List<int> CorectOption { get; set; } // List of indices of correct options
    public QuestionType Type { get; set; }
}

public enum QuestionType
{
    Choice,
    CompleteEmptySpace
}