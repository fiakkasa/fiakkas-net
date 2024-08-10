using ui.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var isDev = builder.Environment.IsDevelopment();

builder.Host.AddUiLoggingProvider();

services.AddUiConfig();
services.AddHtmlParser();

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddFiakkasNetApiClient();
services.AddEmailService();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/404");

// Configure the HTTP request pipeline.
if (!isDev)
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

app.UseStaticFiles();
app.UseAntiforgery();

// note: add serilog after "noisy" middleware
app.UseUiLoggingProvider();

app
    .MapRazorComponents<ui.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
