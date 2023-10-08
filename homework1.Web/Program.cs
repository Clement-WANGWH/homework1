using homework1.Web.Service;
using homework1.Web.Data;
using homework1.Web.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBootstrapBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<ITodayPoetryService, JinrishiciService>();
builder.Services.AddScoped<IPoetryStorage, PoetryStorage>();
builder.Services.AddScoped<IPreferenceStorage, PreferenceStorage>();
builder.Services.AddScoped<IAlertService, AlertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
