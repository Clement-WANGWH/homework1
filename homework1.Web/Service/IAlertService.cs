namespace homework1.Web.Service;

public interface IAlertService
{
    Task AlertAsync(string title, string message, string button);
}