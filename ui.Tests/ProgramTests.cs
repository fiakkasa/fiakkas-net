using Microsoft.AspNetCore.Mvc.Testing;
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

                config.AddToConfigurationBuilder(
"""
{
  "UiConfig": {
    "Title": "UI",
    "Separator": " - "
  },
  "FiakkasNetApiConfig": {
    "BaseUrl": "https://test.com"
  },
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
  "AllowedHosts": "*"
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

        var result = await client.GetAsync("/error");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Program_Should_Run_In_Development_Mode()
    {
        using var app = new Waf("Development");

        var client = app.CreateClient();

        var result = await client.GetAsync("/error");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}