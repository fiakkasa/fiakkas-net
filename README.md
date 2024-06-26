# fiakkas-net

## Overview

The main goal is to have a well defined contracts based implementation with each project under the solution contributing their own assets to the overall solution API.

The solution structure is inspired by the Vertical Slice architecture.

Solution structure:

- api: API
- api.Shared: Shared assets
- api.GraphExtensions: Extends the GraphQL API surface
- api.Tests: Tests!
- api.NodeProxy: A proxy wrapper in node.js [📝](./api.NodeProxy/README.md)
- api.`<Feature>`: GraphQL enabled data domains

All projects are structured and arranged by technical concerns.

## Spinning up the API

### Installation

The .NET SDK can be found at https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

Visual Studio Code can be found at https://code.visualstudio.com

### Trusting the Default ASP.Net Certificate

`dotnet dev-certs https --trust`

### Data

Populate a `data.json` file with data under the `api` project.

Consider using the `data.sample.json` file as a starting point.

### Running the API

- VS Code: use the included profile
- cli: `dotnet run --project ./api/api.csproj --urls https://localhost:7211`

## Try it out!

- BananaCakePop: https://localhost:7211/graphql
- Voyager: https://localhost:7211/voyager

## Testing

The `api.Tests` is using the XUnit framework to test the various aspects of the solution.

To run the tests navigate to the `api.Tests` directory.

Before running the tests for the first time, ensure that you run the `dotnet tool restore` command.

To run the tests, and produce the coverage report run:

`dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./coverage.cobertura.xml'; dotnet reportgenerator -reports:./coverage.cobertura.xml -targetdir:./TestResults -reporttypes:Html`

## Logging

The solution is configured to use the ILogger abstraction with Serilog and most specifically the Console and File sinks.

In addition a number of enrichers are present and enabled by default:

- Serilog.Enrichers.AssemblyName
- Serilog.Enrichers.ClientInfo
- Serilog.Enrichers.Environment
- Serilog.Enrichers.Process
- Serilog.Enrichers.Thread

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
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
      "Default": "Information",
      "Override": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "Properties": {
      "Application": "FiakkasNetApi"
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/api.log",
          "shared": true,
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact",
          "fileSizeLimitBytes": 102400,
          "rollOnFileSizeLimit": true,
          "retainedFileCountLimit": 10
        }
      }
    ]
  }
}
```

## References

- ASP.NET: https://dotnet.microsoft.com/en-us/apps/aspnet
- .NET SDK: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks
- VS Code: https://code.visualstudio.com
- HotChocolate: https://chillicream.com/docs/hotchocolate
- GraphQL: https://graphql.org
- Vertical Slide Architecture: https://github.com/SSWConsulting/SSW.VerticalSliceArchitecture
- Serilog Enrichment: https://github.com/serilog/serilog/wiki/Enrichment
- Serilog.AspNetCore: https://github.com/serilog/serilog-aspnetcore
- Serilog.Enrichers.AssemblyName: https://github.com/TinyBlueRobots/Serilog.Enrichers.AssemblyName
- Serilog.Enrichers.ClientInfo: https://github.com/serilog-contrib/serilog-enrichers-clientinfo
- Serilog.Enrichers.Environment: https://github.com/serilog/serilog-enrichers-environment
- Serilog.Enrichers.Process: https://github.com/serilog/serilog-enrichers-process
- Serilog.Enrichers.Thread: https://github.com/serilog/serilog-enrichers-thread
- Serilog.Formatting.Compact: https://github.com/serilog/serilog-formatting-compact
- Serilog.Settings.Configuration: https://github.com/serilog/serilog-settings-configuration
- Serilog.Sinks.Console: https://github.com/serilog/serilog-sinks-console
- Serilog.Sinks.File: https://github.com/serilog/serilog-sinks-file
