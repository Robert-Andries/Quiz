using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Models;

namespace Quiz.WPFUI.ViewModels;

public class MainViewModel : BaseViewModel
{
    public MainViewModel(INavigationStore navigationStore) : base(navigationStore)
    {
        if (navigationStore.CurrentViewModel == null)
            navigationStore.NavigateTo<HomeViewModel>();
    }
}