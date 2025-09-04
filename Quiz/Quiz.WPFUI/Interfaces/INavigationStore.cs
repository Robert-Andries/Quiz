using Quiz.WPFUI.Models;

namespace Quiz.WPFUI.Interfaces;

/// <summary>
/// Interface for navigation store to manage current view model and navigation.
/// </summary>
public interface INavigationStore
{
    public BaseViewModel CurrentViewModel { get; set; }
    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
}