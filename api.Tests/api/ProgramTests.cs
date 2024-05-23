using HotChocolate.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace api.Tests;

public class ProgramTests
{
    internal class Waf(string environment) : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Sources.Clear();

                config.AddToConfigBuilder(
                    new()
                    {
                        [Consts.DataFileSectionPath] = JsonSerializer.Deserialize<object>(
"""
{
    "portfolioCategories": [
      {
        "id": "45ccaebe-e434-465d-b8a5-c2badaa4132a",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "Category"
      }
    ],
    "customers": [
      {
        "id": "9622e0b5-2597-4015-93cb-120f4783d06a",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "Customer",
        "href": "https://www.customer.com"
      }
    ],
    "technologies": [
      {
        "id": "f4fe386b-0623-4021-a61d-735e7b656ded",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "C#",
        "href": "https://learn.microsoft.com/en-us/dotnet/csharp/"
      }
    ],
    "portfolioItems": [
      {
        "id": "9ee0f303-2798-4d3f-91d6-654d529f7c94",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "year": 2024,
        "categoryId": "45ccaebe-e434-465d-b8a5-c2badaa4132a",
        "ordinal": 169,
        "title": "EPC Data Api",
        "href": null,
        "technologyIds": [
          "0089603d-4574-4533-9515-9ddca3c6efd4"
        ],
        "customerId": "9622e0b5-2597-4015-93cb-120f4783d06a"
      }
    ]
  }
"""
                        )!,
                        ["Logging:LogLevel:Default"] = "Error",
                        ["AllowedHosts"] = "*"
                    }
                );
            });

            builder.UseEnvironment(environment);

            return base.CreateHost(builder);
        }
    }

    [Fact]
    public async Task Program_Should_Run_In_Release_Mode()
    {
        using var app = new Waf("Release");

        var client = app.CreateClient();

        var executor = await app.Server.Services.GetRequestExecutorAsync();
        var schema = executor.Schema.Print();

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();

        indexResult.StatusCode.Should().Be(HttpStatusCode.OK);
        healthResult.StatusCode.Should().Be(HttpStatusCode.OK);
        graphqlResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
        graphqlVisualizerResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Program_Should_Run_In_Development_Mode()
    {
        using var app = new Waf("Development");

        var client = app.CreateClient();

        var executor = await app.Server.Services.GetRequestExecutorAsync();
        var schema = executor.Schema.Print();

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();

        indexResult.StatusCode.Should().Be(HttpStatusCode.OK);
        healthResult.StatusCode.Should().Be(HttpStatusCode.OK);
        graphqlResult.StatusCode.Should().Be(HttpStatusCode.OK);
        graphqlVisualizerResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
