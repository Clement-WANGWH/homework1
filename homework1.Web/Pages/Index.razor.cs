using homework1.Web.Models;
using homework1.Web.Service;

namespace homework1.Web.Pages;

public partial class Index
{
    private TodayPoetry _todayPoetry = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _todayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
        StateHasChanged();
    }
}