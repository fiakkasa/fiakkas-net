# Running .NET within Node.JS

## Overview

The main goal is to be able to run the .NET API through a Node proxy using an in process instantiation in environment
where only Node is supported with limited access to the underlying infra.

The solution was tested using Node v18.20.8 and .NET 9.

## Publishing

When the .NET API is published it should pack all the necessary bits so that it can be used in a standalone fashion at
the designated environment of choice.

Command: `dotnet publish --self-contained --os <os> --arch <architecture>`

Example: `dotnet publish --self-contained --os linux --arch x64`

## Configuration

| Environmental Variable | Description                                                      |
|------------------------|------------------------------------------------------------------|
| PORT                   | The node port; defaults to 3000                                  |
| DOTNET_PROTOCOL        | The communication protocol for the .NET API; defaults to http    |
| DOTNET_PORT            | The .NET API port; defaults to 5001                              |
| DOTNET_ASSETS_PATH     | The path where the publish folder resides; defaults to ./publish |
| DOTNET_EXECUTABLE_NAME | The .NET api executable name; defaults to api                    |

## Starting the Node Proxy and the .NET API

### Installation

The Node runtime can be found at https://nodejs.org/en

Visual Studio Code can be found at https://code.visualstudio.com

Before running ensure that the node modules are installed, `npm i`.

### Running the API

- VS Code: use the included profile
- cli:
    - publish the api: `dotnet publish ../api/api.csproj -o ./publish`
    - run: `node index.js` or `npm start`.

## References

- https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
- https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
