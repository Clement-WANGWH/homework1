using Microsoft.AspNetCore.Components;

namespace homework1.Service;

public class NavigationService : INavigationService
{
    private readonly NavigationManager _navigationManager;

    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void NavigateTo(string uri) => _navigationManager.NavigateTo(uri);

    public void NavigateTo(string uri, object parameter)
    {
        throw new NotImplementedException();
    }
}