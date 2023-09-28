using Microsoft.AspNetCore.Components;

namespace homework1.Pages;

public partial class Detail
{
    [Parameter]
    public string Id
    {
        get;
        set;
    }
}