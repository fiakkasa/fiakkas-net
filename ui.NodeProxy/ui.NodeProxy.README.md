# Running .NET within Node.JS

## Overview

The main goal is to be able to run the .NET UI through a Node proxy using an in process instantiation in environment
where only Node is supported with limited access to the underlying infra.

The solution was tested using Node v20.12.2 and .NET 9.

## Publishing

When the .NET UI is published it should pack all the necessary bits so that it can be used in a standalone fashion at
the designated environment of choice.

Command: `dotnet publish --self-contained --os <os> --arch <architecture>`

Example: `dotnet publish --self-contained --os linux --arch x64`

## Configuration

| Environmental Variable    | Description                                                      |
|---------------------------|------------------------------------------------------------------|
| PORT                      | The node port; defaults to 3000                                  |
| DOTNET_PROTOCOL           | The communication protocol for the .NET UI; defaults to http     |
| DOTNET_PORT               | The .NET UI port; defaults to 5000                               |
| DOTNET_UI_FOLDER_PATH     | The path where the publish folder resides; defaults to ./publish |
| DOTNET_UI_EXECUTABLE_NAME | The .NET ui executable name; defaults to ui                      |

## Starting the Node Proxy and the .NET UI

### Installation

The Node runtime can be found at https://nodejs.org/en

Visual Studio Code can be found at https://code.visualstudio.com

Before running ensure that the node modules are installed, `npm i`.

### Running the UI

- VS Code: use the included profile
- cli:
    - publish the ui: `dotnet publish ../ui/ui.csproj -o ./publish`
    - run: `node index.js` or `npm start`.

## References

- https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
- https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
