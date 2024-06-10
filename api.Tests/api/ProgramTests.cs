using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Net;

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
"""
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console"
    ],
    "Enrich": [
      "WithClientIp",
      {
        "Name": "WithRequestHeader",
        "Args": {
          "headerName": "User-Agent"
        }
      },
      {
        "Name": "WithRequestHeader",
        "Args": {
          "headerName": "Connection"
        }
      },
      {
        "Name": "WithRequestHeader",
        "Args": {
          "headerName": "Content-Length",
          "propertyName": "RequestLength"
        }
      },
      {
        "Name": "WithCorrelationId",
        "Args": {
          "headerName": "x-correlation-id",
          "addValueIfHeaderAbsence": true
        }
      },
      "WithMachineName",
      "WithEnvironmentUserName",
      "WithEnvironmentName",
      "WithProcessId",
      "WithProcessName",
      "WithThreadId",
      "WithThreadName",
      "WithAssemblyInformationalVersion"
    ],
    "MinimumLevel": {
      // "Verbose", "Debug", "Information", "Warning", "Error", "Fatal"
      "Default": "Fatal",
      "Override": {
        "Default": "Fatal",
        "Microsoft.AspNetCore": "Fatal"
      }
    },
    "Properties": {
      "Application": "FiakkasNetApi"
    },
    "WriteTo": [
      {
        "Name": "Console"
      }
    ]
  },
  "AllowedHosts": "*",
  "data": {
    "categories": [
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
        "id": "0089603d-4574-4533-9515-9ddca3c6efd4",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "Angular",
        "href": "https://angular.io"
      },
      {
        "id": "480a46c6-5047-4ef6-b319-6378056a6191",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "ASP.NET (Modern)",
        "href": "https://www.asp.net"
      },
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
        "title": "Data Api",
        "href": null,
        "technologyIds": [
          "0089603d-4574-4533-9515-9ddca3c6efd4",
          "480a46c6-5047-4ef6-b319-6378056a6191",
          "f4fe386b-0623-4021-a61d-735e7b656ded"
        ],
        "customerId": "9622e0b5-2597-4015-93cb-120f4783d06a"
      }
    ],
    "textItems": [
      {
        "id": "2f69e973-550b-4769-801a-e757807e6845",
        "createdAt": "2024-06-07T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "key": "test",
        "title": "Test",
        "content": "Exercitation laborum ad occaecat ut adipisicing."
      }
    ],
    "languages": [
      {
        "id": "02a3be9b-3f04-4b4a-8945-e84fef537b58",
        "createdAt": "2024-06-10T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "English",
        "proficiency": "Native"
      }
    ]
  }
}
"""
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

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

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

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

        indexResult.StatusCode.Should().Be(HttpStatusCode.OK);
        healthResult.StatusCode.Should().Be(HttpStatusCode.OK);
        graphqlResult.StatusCode.Should().Be(HttpStatusCode.OK);
        graphqlVisualizerResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
