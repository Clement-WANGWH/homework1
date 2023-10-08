using homework1.Web.Models;

namespace homework1.Web.Service;

public interface ITodayPoetryService
{
    Task<TodayPoetry> GetTodayPoetryAsync();
}

public static class TodayPoetrySources
{
    public const string Jinrishici = nameof(Jinrishici);
    public const string Local = nameof(Local);
}