using Quiz.WPFUI.Interfaces;
using Quiz.WPFUI.Models;
using Quiz.WPFUI.Utilities;

namespace Quiz.WPFUI.Stores;

public class NavigationStore : ObservableObject, INavigationStore
{
    private readonly Func<Type, BaseViewModel> _getViewModel;
    private BaseViewModel _currentViewModel = default!;
    
    public NavigationStore(Func<Type, BaseViewModel> getViewModel)
    {
        this._getViewModel = getViewModel;
    }
    
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }
    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        CurrentViewModel = _getViewModel?.Invoke(typeof(TViewModel))!;
    }
}