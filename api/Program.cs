using api;
using api.Application.Extensions;
using api.Customers.Extensions;
using api.Extensions;
using api.Portfolio.Extensions;
using api.Technologies.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var isDev = builder.Environment.IsDevelopment();

config.AddJsonFile(Consts.DataFile, reloadOnChange: true, optional: true);

services.AddApiApplication(typeof(Program).Assembly);
services.AddApiCustomers(config, Consts.DataFileSectionPath);
services.AddApiPortfolio(config, Consts.DataFileSectionPath);
services.AddApiTechnologies(config, Consts.DataFileSectionPath);

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

app.UseHttpsRedirection();

app.UseApiCors();

app.MapApiGraphQL(isDev);

app.Run();
