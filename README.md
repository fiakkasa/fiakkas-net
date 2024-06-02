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

## References

- ASP.NET: https://dotnet.microsoft.com/en-us/apps/aspnet
- .NET SDK: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks
- VS Code: https://code.visualstudio.com
- HotChocolate: https://chillicream.com/docs/hotchocolate
- GraphQL: https://graphql.org
- Vertical Slide Architecture: https://github.com/SSWConsulting/SSW.VerticalSliceArchitecture
