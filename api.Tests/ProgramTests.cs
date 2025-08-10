namespace api.Tests;

public class ProgramTests
{
    private const string _configurationDefinition =
"""
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console"
    ],
    "Enrich": [
      "FromLogContext",
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
        "kind": "Portfolio",
        "id": "45ccaebe-e434-465d-b8a5-c2badaa4132a",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "Category"
      },
      {
        "kind": "SoftwareDevelopment",
        "id": "f4fe386b-0623-4021-a61d-735e7b656ded",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "C#",
        "href": "https://learn.microsoft.com/en-us/dotnet/csharp/"
      },
      {
        "kind": "Resume",
        "id": "eb9d6258-99c4-46bd-bd44-23d35b19965d",
        "createdAt": "2024-05-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "title": "Software Development",
        "associatedCategoryTypes": [
          "SoftwareDevelopment"
        ]
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
    ],
    "contactItems": [
      {
        "id": "ebf224a8-7ff3-47b9-882b-dd41ec7f5a05",
        "createdAt": "2024-06-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "key": "email",
        "icon": "envelope",
        "title": "Email",
        "content": "user@email.com",
        "href": "mailto:user@email.com"
      }
    ],
    "achievements": [
      {
        "id": "d4605b0c-58bc-49ac-bcfd-10a24a203add",
        "createdAt": "2024-06-15T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "content": "Some great achievement!",
        "years": [
          2024
        ]
      }
    ],
    "educationItems": [
      {
        "id": "38898c62-161e-40f2-8a9f-39bf1ff46224",
        "createdAt": "2024-07-08T00:00:00.000Z",
        "updatedAt": null,
        "version": 0,
        "categoryId": "eb9d6258-99c4-46bd-bd44-23d35b19965d",
        "timePeriod": {
          "start": "2000-01-01",
          "end": "2004-06-01"
        },
        "title": "Example University",
        "href": null,
        "location": "Dallas Texas, USA",
        "description": "Bachelors of Computer Science",
        "subjects": [
          "Programming",
          "Database Design"
        ]
      }
    ]
  }
}
""";

    [Fact]
    public async Task Program_Should_Run_In_Release_Mode()
    {
        await using var app = new Waf("Release");

        var client = app.CreateClient();

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

        Assert.Equal(HttpStatusCode.OK, indexResult.StatusCode);
        Assert.Equal(HttpStatusCode.OK, healthResult.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, graphqlResult.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, graphqlVisualizerResult.StatusCode);
    }

    [Fact]
    public async Task Program_Should_Run_In_Development_Mode()
    {
        await using var app = new Waf("Development");

        var client = app.CreateClient();

        var indexResult = await client.GetAsync("index.html");
        var healthResult = await client.GetAsync(Consts.HealthEndPoint);
        var graphqlResult = await client.GetAsync(Consts.GraphQLEndPoint);
        var graphqlVisualizerResult = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);

        Assert.Equal(HttpStatusCode.OK, indexResult.StatusCode);
        Assert.Equal(HttpStatusCode.OK, healthResult.StatusCode);
        Assert.Equal(HttpStatusCode.OK, graphqlResult.StatusCode);
        Assert.Equal(HttpStatusCode.OK, graphqlVisualizerResult.StatusCode);
    }

    private class Waf(string environment) : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
                config.AddToConfigurationBuilder(_configurationDefinition)
            );
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.Sources.Clear();

                config.AddToConfigurationBuilder(_configurationDefinition);
            });

            builder.UseEnvironment(environment);

            return base.CreateHost(builder);
        }
    }
}
