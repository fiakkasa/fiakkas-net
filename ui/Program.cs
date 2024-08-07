using ui.Components;
using ui.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var isDev = builder.Environment.IsDevelopment();

builder.Host.AddUiLoggingProvider();

services.AddUiConfig();

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddFiakkasNetApiClient();

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
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
