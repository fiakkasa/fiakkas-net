using ui.Components;
using ui.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var isDev = builder.Environment.IsDevelopment();

builder.Host.AddUiLoggingProvider();

services.AddUiConfig();
services.AddUiCache();
services.AddHtmlParser();

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddFiakkasNetApiClient();
services.AddEmailService();

services.AddResponseCompression();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/404");

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!isDev)
{
    app.UseExceptionHandler("/Error", true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// note: add serilog after "noisy" middleware
app.UseUiLoggingProvider();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
