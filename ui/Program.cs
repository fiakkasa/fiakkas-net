using ui.Components;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var isDev = builder.Environment.IsDevelopment();

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!isDev)
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

app.UseStaticFiles();
app.UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
