using api.Extensions;
using HotChocolate.Execution;
using HotChocolate.Types.Relay;

namespace api.Tests.Extensions;

public class GraphQLExtensionsTests
{
    [Fact]
    public async Task AddApiGraphQL_Should_Register_The_GraphQL_Server_And_Graph_Endpoints()
    {
        var requestExecutor =
            await new ServiceCollection()
                .AddApiGraphQL(false)
                .BuildServiceProvider()
                .GetRequestExecutorAsync();

        var schema = requestExecutor.Schema.Print();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();
    }

    [Fact]
    public async Task AddApiGraphQLServer_And_MapApiGraphQL_In_Dev_Mode_Should_Add_GraphQL_Server_And_Map_Endpoints()
    {
        const bool isDev = true;
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
                services
                    .AddRouting()
                    .AddApiGraphQLServer(isDev)
                    .AddTypeExtension(typeof(TestQueries))
            )
            .Configure(app =>
                app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapApiGraphQL(isDev))
            );

        using var server = new TestServer(builder);
        var graph = await server.Services.GetRequestExecutorAsync();
        var client = server.CreateClient();

        var schema = graph.Schema.Print();
        var voyagerResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var bananaCakePopResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var queryRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new(Consts.GraphQLEndPoint, UriKind.Relative),
            Content = JsonContent.Create(new
            {
                query = "{ text }"
            })
        };
        var queryResponse = await client.SendAsync(queryRequest);
        var queryResult = await queryResponse.Content.ReadFromJsonAsync<JsonDocument>();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();

        // voyager
        voyagerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        // banana cake pop
        bananaCakePopResponse.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        // query
        queryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        queryResult.Should().NotBeNull();
        queryResult!.RootElement.GetProperty("data").GetProperty("text").GetString().Should().Be("Hello");
    }

    [Fact]
    public async Task
        AddApiGraphQLServer_And_MapApiGraphQL_In_Release_Mode_Should_Add_GraphQL_Server_And_Map_Endpoints()
    {
        const bool isDev = false;
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
                services
                    .AddRouting()
                    .AddApiGraphQLServer(isDev)
                    .AddTypeExtension(typeof(TestQueries))
            )
            .Configure(app =>
                app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapApiGraphQL(isDev))
            );

        using var server = new TestServer(builder);
        var graph = await server.Services.GetRequestExecutorAsync();
        var client = server.CreateClient();

        var schema = graph.Schema.Print();
        var voyagerResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var bananaCakePopResponse = await client.GetAsync(Consts.GraphQLSchemaVisualizerEndPoint);
        var queryRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new(Consts.GraphQLEndPoint, UriKind.Relative),
            Content = JsonContent.Create(new
            {
                query = "{ text }"
            })
        };
        var queryResponse = await client.SendAsync(queryRequest);
        var queryResult = await queryResponse.Content.ReadFromJsonAsync<JsonDocument>();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();

        // voyager
        voyagerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        // banana cake pop
        bananaCakePopResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        // query
        queryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        queryResult.Should().NotBeNull();
        queryResult!.RootElement.GetProperty("data").GetProperty("text").GetString().Should().Be("Hello");
        queryResult.RootElement.TryGetProperty("extensions", out _).Should().BeFalse();
    }

    [Node]
    public record Message(
        [property: ID]
        Guid Id,
        string Text
    );

    [QueryType]
    public static class TestQueries
    {
        private static readonly Message[] _messages =
        [
            new(new("f5b0d13a-86bc-4b89-891c-625672dbbd84"), "hello"),
            new(new("8c34e916-5efd-4485-a46a-6b11383a4deb"), "world")
        ];

        public static string Text => "Hello";

        [UsePaging]
        [UseSorting]
        [UseFiltering]
        public static IEnumerable<Message> GetMessages() => _messages;

        [NodeResolver]
        public static Message? GetMessageById(Guid id) =>
            Array.Find(_messages, m => m.Id == id);
    }
}
