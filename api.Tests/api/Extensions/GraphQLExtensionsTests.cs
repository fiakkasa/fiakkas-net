using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace api.Extensions.Tests;

public class GraphQLExtensionsTests
{
    public record Message(string Text);

    [ExtendObjectType(OperationType.Query)]
    public class TestQueries
    {
        private static readonly Message[] _messages = [new("hello"), new("world")];

        [UseOffsetPaging]
        [UseSorting]
        [UseFiltering]
        public IEnumerable<Message> GetMessages() => _messages;

        public string Text => "Hello";
    }

    [Fact]
    public async Task AddGraphQL_In_Dev_Mode()
    {
        var isDev = true;
        var builder = new WebHostBuilder()
           .ConfigureServices(services =>
                services
                    .AddRouting()
                    .AddApiGraphQLServer(isDev)
                    .AddTypeExtension<TestQueries>()
            )
           .Configure(app =>
                app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapApiGraphQL(isDev))
            );

        using var server = new TestServer(builder);
        var graph = await server.Services.GetRequestExecutorAsync();
        var client = server.CreateClient();

        var schema = graph?.Schema?.Print() ?? string.Empty;
        var voyagerResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var bananaCakePopResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var queryRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new(Consts.GraphQLEndPoint, UriKind.Relative),
            Content = JsonContent.Create(new { query = "{ text }" })
        };
        queryRequest.Headers.Add("GraphQL-Tracing", "1");
        var queryResponse = await client.SendAsync(queryRequest);
        var queryResult = await queryResponse.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();

        // voyager
        Assert.Equal(HttpStatusCode.OK, voyagerResponse.StatusCode);
        // banana cake pop
        Assert.NotEqual(HttpStatusCode.NotFound, bananaCakePopResponse.StatusCode);
        // query
        Assert.Equal(HttpStatusCode.OK, queryResponse.StatusCode);
        Assert.NotNull(queryResult);
        Assert.Equal("Hello", queryResult.RootElement.GetProperty("data").GetProperty("text").GetString());
        Assert.NotEmpty(queryResult.RootElement.GetProperty("extensions").GetProperty("tracing").GetRawText());
    }
    [Fact]
    public async Task AddGraphQL_In_Release_Mode()
    {
        var isDev = false;
        var builder = new WebHostBuilder()
           .ConfigureServices(services =>
                services
                    .AddRouting()
                    .AddApiGraphQLServer(isDev)
                    .AddTypeExtension<TestQueries>()
            )
           .Configure(app =>
                app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapApiGraphQL(isDev))
            );

        using var server = new TestServer(builder);
        var graph = await server.Services.GetRequestExecutorAsync();
        var client = server.CreateClient();

        var schema = graph?.Schema?.Print() ?? string.Empty;
        var voyagerResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var bananaCakePopResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var queryRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new(Consts.GraphQLEndPoint, UriKind.Relative),
            Content = JsonContent.Create(new { query = "{ text }" })
        };
        queryRequest.Headers.Add("GraphQL-Tracing", "1");
        var queryResponse = await client.SendAsync(queryRequest);
        var queryResult = await queryResponse.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();

        // voyager
        Assert.Equal(HttpStatusCode.NotFound, voyagerResponse.StatusCode);
        // banana cake pop
        Assert.Equal(HttpStatusCode.NotFound, bananaCakePopResponse.StatusCode);
        // query
        Assert.Equal(HttpStatusCode.OK, queryResponse.StatusCode);
        Assert.NotNull(queryResult);
        Assert.Equal("Hello", queryResult.RootElement.GetProperty("data").GetProperty("text").GetString());
        Assert.False(queryResult.RootElement.TryGetProperty("extensions", out var _));
    }
}
