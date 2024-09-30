using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace ui.Tests;

public class ProgramTests
{
    [Fact]
    public async Task Program_Should_Run_In_Release_Mode()
    {
        await using var app = new Waf("Release");

        var client = app.CreateClient();

        var result = await client.GetAsync("/error");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Program_Should_Run_In_Development_Mode()
    {
        await using var app = new Waf("Development");

        var client = app.CreateClient();

        var result = await client.GetAsync("/error");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private class Waf(string environment) : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.Sources.Clear();

                config.AddToConfigurationBuilder(
                    """
                    {
                      "UiConfig": {
                        "Title": "UI",
                        "Separator": " - ",
                        "Description": "Description",
                        "Keywords": "Keywords",
                        "Author": "Author"
                      },
                      "FiakkasNetApiConfig": {
                        "BaseUrl": "https://test.com"
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
}
