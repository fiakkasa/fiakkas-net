using api;
using api.Application.Extensions;
using api.Categories.Extensions;
using api.Customers.Extensions;
using api.Extensions;
using api.Portfolio.Extensions;
using api.Technologies.Extensions;
using api.TextItems.Extensions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var isDev = builder.Environment.IsDevelopment();
var versionAttribute = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

builder.Host.UseSerilog((context, configuration) =>
    configuration
        .Enrich.WithProperty(Consts.LogPropertyAppVersion, versionAttribute!.InformationalVersion)
        .ReadFrom.Configuration(context.Configuration)
);

config.AddJsonFile(Consts.DataFile, reloadOnChange: true, optional: true);

services.AddHttpContextAccessor();

services.AddApiApplication(versionAttribute);
services.AddApiCategories(config, Consts.DataFileSectionPath);
services.AddApiCustomers(config, Consts.DataFileSectionPath);
services.AddApiPortfolio(config, Consts.DataFileSectionPath);
services.AddApiTechnologies(config, Consts.DataFileSectionPath);
services.AddApiTextItems(config, Consts.DataFileSectionPath);

services.AddCors();

services
    .AddApiGraphQLServer(isDev)
    .AddApiGraphQLEndpoints();

services.AddApiHealth();

services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapHealthChecks(Consts.HealthEndPoint);

// note: add serilog after "noisy" middleware
app.UseSerilogRequestLogging();

app.UseApiCors();

app.MapApiGraphQL(isDev);

app.Run();
