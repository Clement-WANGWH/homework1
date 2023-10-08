using homework1.Web.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace homework1.Web.Service;

public class JinrishiciService: ITodayPoetryService
{
    private readonly IAlertService _alertService;

    private readonly IPreferenceStorage _preferenceStorage;

    private readonly IPoetryStorage _poetryStorage;

    public static readonly string JinrishiciTokenKey =
        $"{nameof(JinrishiciService)}.Token";

    private const string Server = "今日诗词服务器";

    public JinrishiciService(IAlertService alertService,
        IPreferenceStorage preferenceStorage, IPoetryStorage poetryStorage)
    {
        _alertService = alertService;
        _preferenceStorage = preferenceStorage;
        _poetryStorage = poetryStorage;
    }

    public async Task<TodayPoetry> GetTodayPoetryAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return await GetRandomPoetryAsync();
        }

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-User-Token", token);

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.GetAsync("https://v2.jinrishici.com/sentence");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return await GetRandomPoetryAsync();
        }

        var json = await response.Content.ReadAsStringAsync();
        JinrishiciSentence jinrishiciSentence;
        try
        {
            jinrishiciSentence = JsonSerializer.Deserialize<JinrishiciSentence>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(
                ErrorMessages.JsonDeserializationErrorTitle,
                ErrorMessages.GetJsonDeserializationError(Server, e.Message),
                ErrorMessages.JsonDeserializationErrorButton);
            return await GetRandomPoetryAsync();
        }

        return new TodayPoetry
        {
            Snippet = jinrishiciSentence.Data.Content,
            Name = jinrishiciSentence.Data.Origin.Title,
            Dynasty = jinrishiciSentence.Data.Origin.Dynasty,
            Author = jinrishiciSentence.Data.Origin.Author,
            Content = string.Join(Environment.NewLine,
                jinrishiciSentence.Data.Origin.Content),
            Source = TodayPoetrySources.Jinrishici
        };
    }

    public async Task<TodayPoetry> GetRandomPoetryAsync()
    {
        var poetries = await _poetryStorage.GetPoetriesAsync(
            // p => true
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")),
            new Random().Next(30), 1);
        var poetry = poetries.First();
        return new TodayPoetry
        {
            Snippet = poetry.Snippet,
            Name = poetry.Name,
            Dynasty = poetry.Dynasty,
            Author = poetry.Author,
            Content = poetry.Content,
            Source = TodayPoetrySources.Local
        };
    }

    private string _token = string.Empty;

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_token))
        {
            return _token;
        }

        _token = _preferenceStorage.Get(JinrishiciTokenKey, string.Empty);
        if (!string.IsNullOrWhiteSpace(_token))
        {
            return _token;
        }

        using var httpClient = new HttpClient();

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.GetAsync("https://v2.jinrishici.com/token");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return _token;
        }


        var json = await response.Content.ReadAsStringAsync();

        JinrishiciToken jinrishiciToken;
        try
        {
            jinrishiciToken = JsonSerializer.Deserialize<JinrishiciToken>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(
                ErrorMessages.JsonDeserializationErrorTitle,
                ErrorMessages.GetJsonDeserializationError(Server, e.Message),
                ErrorMessages.JsonDeserializationErrorButton);
            return _token;
        }

        _token = jinrishiciToken.Data;
        _preferenceStorage.Set(JinrishiciTokenKey, _token);
        return _token;
    }
}

public class JinrishiciToken
{
    public string Status { get; set; }
    public string Data { get; set; }
}

public class JinrishiciSentence
{
    public JinrishiciData Data { get; set; }
}

public class JinrishiciData
{
    public string Content { get; set; }
    public JinrishiciOrigin Origin { get; set; }
}

public class JinrishiciOrigin
{
    public string Title { get; set; }
    public string Dynasty { get; set; }
    public string Author { get; set; }
    public string[] Content { get; set; }
}