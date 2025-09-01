using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Utilities;
using Quiz.WPFUI.ViewModels;

namespace Quiz.WPFUI.Models;

public class BaseViewModel : ObservableObject
{
    public INavigationStore NavigationStore { get; }
    
    public BaseViewModel(INavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
    }
}