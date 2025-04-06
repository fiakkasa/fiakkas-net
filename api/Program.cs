using api;
using api.Extensions;

var start = DateTimeOffset.Now;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var isDev = builder.Environment.IsDevelopment();

builder.Host.AddAppLoggingProvider();

config.AddJsonFile(Consts.DataFile, reloadOnChange: true, optional: true);

services.AddHttpContextAccessor();

services.AddApiAchievements(Consts.DataFileSectionPath);
services.AddApiApplication(start, typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>());
services.AddApiCategories(Consts.DataFileSectionPath);
services.AddApiContactItems(Consts.DataFileSectionPath);
services.AddApiCustomers(Consts.DataFileSectionPath);
services.AddApiEducationItems(Consts.DataFileSectionPath);
services.AddApiLanguages(Consts.DataFileSectionPath);
services.AddApiPortfolio(Consts.DataFileSectionPath);
services.AddApiTextItems(Consts.DataFileSectionPath);

services.AddCors();

services.AddApiGraphQL(isDev);

services.AddApiHealth();

services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapHealthChecks(Consts.HealthEndPoint);

// note: add serilog after "noisy" middleware
app.UseAppLoggingProvider();

app.UseApiCors();

app.MapApiGraphQL(isDev);

app.RunWithGraphQLCommands(args);
