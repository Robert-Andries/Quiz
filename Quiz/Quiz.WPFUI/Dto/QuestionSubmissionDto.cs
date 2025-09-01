using Quiz.WPFUI.Utilities;

namespace Quiz.WPFUI.Dto;

public class QuestionSubmissionDto : ObservableObject
{
    private string _optionText;
    private bool _isChecked;

    public string OptionText
    {
        get => _optionText;
        set
        {
            if (value == _optionText) return;
            _optionText = value;
            OnPropertyChanged();
        }
    }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (value == _isChecked) return;
            _isChecked = value;
            OnPropertyChanged();
        }
    }
}