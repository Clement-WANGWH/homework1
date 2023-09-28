namespace homework1.Service;

public interface INavigationService
{
    void NavigateTo(string uri);

    void NavigateTo(string uri, object parameter);
}

public static class NavigationServiceConstants
{
    public const string DetailPage = "/detail";
}