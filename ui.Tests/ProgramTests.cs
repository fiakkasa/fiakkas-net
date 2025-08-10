using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace ui.Tests;

public class ProgramTests
{
    private const string _configurationDefinition =
"""
{
  // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
  "ForwardedHeadersConfig": {
    "Enable": true,
    "ForwardedForHeaderName": "X-Forwarded-For",
    "ForwardedHostHeaderName": "X-Forwarded-Host",
    "ForwardedProtoHeaderName": "X-Forwarded-Proto",
    "ForwardedPrefixHeaderName": "X-Forwarded-Prefix",
    "OriginalForHeaderName": "X-Original-For",
    "OriginalHostHeaderName": "X-Original-Host",
    "OriginalProtoHeaderName": "X-Original-Proto",
    "OriginalPrefixHeaderName": "X-Original-Prefix",
    "ForwardedHeaders": "XForwardedFor, XForwardedProto",
    "ForwardLimit": 1,
    "KnownProxies": [
      "::1"
    ],
    "KnownNetworks": [
      {
        "Address": "127.0.0.0",
        "PrefixLength": 8
      }
    ],
    "AllowedHosts": [],
    "RequireHeaderSymmetry": false
  },
  "UiConfig": {
    "Title": "UI",
    "Separator": " - ",
    "Description": "Description",
    "Keywords": "Keywords",
    "Author": "Author",
    "FullScreenLoaderTransitionDelay": 334,
    "FullScreenLoaderTransitionDuration": 667,
    "UseCompatibilityTransport": false
  },
  "FiakkasNetApiConfig": {
    "BaseUrl": "https://test.com",
    // Exponential, Linear, Constant
    "DelayBackoffType": "Exponential",
    "UseJitter": true,
    "MaxRetryAttempts": 3,
    "Delay": "00:00:00.200"
  },
  "SmtpConfig": {
    "Host": "smtp-host",
    "Port": 25,
    "EnableSsl": false
  },
  "EmailConfig": {
    "AlwaysUseDefaultSenderAddress": false,
    "DefaultSenderAddress": "email@user.com",
    "DefaultRecipientAddress": "email@user.com",
    "PlainTextSignature": "Hello!",
    "HtmlSignature": "Hello!"
  },
  // https://github.com/serilog/serilog-settings-configuration
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.File"
    ],
    "Enrich": [
      "FromLogContext",
      "WithClientIp",
      {
        "Name": "WithCorrelationId",
        "Args": {
          "headerName": "x-correlation-id",
          "addValueIfHeaderAbsence": true
        }
      },
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
      "Default": "Information",
      "Override": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "Properties": {
      "Application": "FiakkasNetUi"
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/ui.log",
          "shared": true,
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact",
          "fileSizeLimitBytes": 102400,
          "rollOnFileSizeLimit": true,
          "retainedFileCountLimit": 10
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
""";

    [Fact]
    public async Task Program_Should_Run_In_Release_Mode()
    {
        await using var app = new Waf("Release");

        var client = app.CreateClient();

        var result = await client.GetAsync("/error");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Program_Should_Run_In_Development_Mode()
    {
        await using var app = new Waf("Development");

        var client = app.CreateClient();

        var result = await client.GetAsync("/error");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    private class Waf(string environment) : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
                config.AddToConfigurationBuilder(_configurationDefinition)
            );
            builder.ConfigureAppConfiguration(config =>
            {
                config.Sources.Clear();

                config.AddToConfigurationBuilder(_configurationDefinition);

                if (environment == "Release")
                {
                    config.AddToConfigurationBuilder("""{ "UiConfig": { "UseCompatibilityTransport": true } }""");
                }
            });

            builder.UseEnvironment(environment);

            return base.CreateHost(builder);
        }
    }
}
