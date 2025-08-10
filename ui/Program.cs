using ui.Components;
using ui.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;
var isDev = builder.Environment.IsDevelopment();

builder.Host.AddAppLoggingProvider();

services.AddHttpContextAccessor();

services.AddValidatedOptions<ForwardedHeadersConfig>();
services.AddUiConfig();
services.AddUiCache();
services.AddHtmlParser();

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddFiakkasNetApiClient(config);
services.AddEmailService();

services.AddResponseCompression();

var app = builder.Build();

app.UseAppForwardedHeaders();

app.UseStatusCodePagesWithRedirects("/404");

// Configure the HTTP request pipeline.
if (!isDev)
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error", true);
}

app.MapStaticAssets();
app.UseAntiforgery();

// note: add serilog after "noisy" middleware
app.UseAppLoggingProvider();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
