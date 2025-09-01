using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Utilities;
using Quiz.WPFUI.ViewModels;

namespace Quiz.WPFUI.Models;

/// <summary>
/// Class representing the base view model with navigation capabilities.
/// Used for inheriting common functionality in other view models.
/// </summary>
public class BaseViewModel : ObservableObject
{
    public INavigationStore NavigationStore { get; }
    
    public BaseViewModel(INavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
    }
}