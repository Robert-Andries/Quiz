using Quiz.WPFUI.Models;

namespace Quiz.WPFUI.Interfaces;

public interface INavigationStore
{
    public BaseViewModel? CurrentViewModel { get; set; }
    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
}