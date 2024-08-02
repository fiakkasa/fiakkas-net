# fiakkas-net

## Table of Contents

- [fiakkas-net](#fiakkas-net)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Spinning up the API or the UI](#spinning-up-the-api-or-the-ui)
    - [Installation](#installation)
    - [Trusting the Default ASP.Net Certificate](#trusting-the-default-aspnet-certificate)
    - [API Data](#api-data)
    - [Running the API](#running-the-api)
      - [Try it out!](#try-it-out)
    - [UI Configuration](#ui-configuration)
    - [UI to API GraphQL Client](#ui-to-api-graphql-client)
      - [Init Strawberry Shake](#init-strawberry-shake)
    - [Running the UI](#running-the-ui)
      - [Try it out!](#try-it-out-1)
  - [Testing](#testing)
  - [Logging](#logging)
  - [Exporting the Schema](#exporting-the-schema)
  - [References](#references)

## Overview

The main goal is to have a well defined contracts based implementation with each project under the solution contributing their own assets to the overall solution.

The solution structure is inspired by the Vertical Slice architecture.

Solution structure:

- api: API
- api.`<Feature>`: GraphQL enabled data domains
- api.`<Feature>`.Tests: Tests!
- api.GraphExtensions: Extends the GraphQL API surface
- api.NodeProxy: A proxy wrapper in node.js [📝](./api.NodeProxy/README.md)
- api.Shared: Shared API assets
- api.Tests: Tests!
- ui: UI
- ui.NodeProxy: A proxy wrapper in node.js [📝](./ui.NodeProxy/README.md)

All projects are structured and arranged by technical concerns.

## Spinning up the API or the UI

### Installation

The .NET SDK can be found at https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

Visual Studio Code can be found at https://code.visualstudio.com

### Trusting the Default ASP.Net Certificate

`dotnet dev-certs https --trust`

### API Data

Populate a `data.json` file with data under the `api` project.

Consider using the `data.sample.json` file as a starting point.

### Running the API

- VS Code: use the included profile
- cli: `dotnet run --project ./api/api.csproj --urls https://localhost:7211`

#### Try it out!

- BananaCakePop: https://localhost:7211/graphql
- Voyager: https://localhost:7211/voyager

### UI Configuration

```json
{
  "UiConfig": {
    "Title": "<Title>",
    "Separator": " - "
  },
  "FiakkasNetApiConfig": {
    "BaseUrl": "api_url/graphql"
  }
}
```

### UI to API GraphQL Client

#### Init Strawberry Shake

```bash
dotnet graphql init http://localhost:5069/graphql -n FiakkasNetApi -p ./FiakkasNetApi
```

### Running the UI

- VS Code: use the included profile
- cli: `dotnet run --project ./ui/ui.csproj --urls https://localhost:7211`

#### Try it out!

- UI: https://localhost:7296/graphql

## Testing

Each test project, ex `api.Tests`, is using the XUnit framework to test the various aspects of the code.

Notably, the `ui.Tests` project is using the BUnit framework in conjunction with XUnit.

All the related tooling is installed under the root folder of the solution.

Before running the tests for the first time, ensure that you run the `dotnet tool restore` command.

To run all tests, merge the produced coverage assets, and produce a coverage report run:

_Bash_

```bash
rm -rd ./CoverageResults
rm -rd ./TestResults
dotnet test /p:CollectCoverage=true /p:CoverletOutput=../CoverageResults/ /p:MergeWith="../CoverageResults/coverage.json" /p:CoverletOutputFormat=\"cobertura,json\" /p:Exclude="[*]*GraphRequestExecutorBuilderExtensions" /p:ExcludeByAttribute="GeneratedCodeAttribute" -m:1
dotnet reportgenerator -reports:./CoverageResults/coverage.cobertura.xml -targetdir:./TestResults -reporttypes:Html
```

To run tests for a specific api related project, enter the `*.Tests` counterpart and run:

_Bash_

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./coverage.cobertura.xml' /p:Exclude="[*]*GraphRequestExecutorBuilderExtensions" /p:ExcludeByAttribute="GeneratedCodeAttribute"
dotnet reportgenerator -reports:./coverage.cobertura.xml -targetdir:./TestResults -reporttypes:Html
```

📝 _Observe the `/p:Exclude="[*]*GraphRequestExecutorBuilderExtensions`, this is added as to ensure that the auto generated HotChocolate registrations are skipped._

📝 _Observe the `/p:ExcludeByAttribute="GeneratedCodeAttribute`, this is added as to ensure that any auto generated code is skipped._

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
      "Application": "<FiakkasNetApi or FiakkasNetUI>"
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/<api or ui>.log",
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

## Exporting the Schema

The schema can be exported by running the api with the following command:

`dotnet run --project ./api/api.csproj -- schema export --output ../schema.graphql`

## References

- .NET SDK: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks
- ASP.NET: https://dotnet.microsoft.com/en-us/apps/aspnet
- BUnit: https://bunit.dev/index.html
- Coverlet (MSBuild): https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/MSBuildIntegration.md
- GraphQL: https://graphql.org
- HotChocolate: https://chillicream.com/docs/hotchocolate
- Report Generator: https://reportgenerator.io
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
- StrawberryShake: https://chillicream.com/docs/strawberryshake/v13/get-started/console
- Vertical Slice Architecture: https://github.com/SSWConsulting/SSW.VerticalSliceArchitecture
- VS Code: https://code.visualstudio.com
- XUnit: https://xunit.net
