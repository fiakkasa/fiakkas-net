using HotChocolate.Execution;

namespace api.Extensions.Tests;

public class GraphQLExtensionsTests
{
    public record Message(string Text);

    [QueryType]
    public static class TestQueries
    {
        private static readonly Message[] _messages = [new("hello"), new("world")];

        [UseOffsetPaging]
        [UseSorting]
        [UseFiltering]
        public static IEnumerable<Message> GetMessages() => _messages;

        public static string Text => "Hello";
    }

    [Fact]
    public async Task AddApiGraphQL_Should_Register_The_GraphQL_Server_And_Graph_Endpoints()
    {
        var requestExecutor =
            await new ServiceCollection()
                .AddApiGraphQL(isDev: false)
                .BuildServiceProvider()
                .GetRequestExecutorAsync();

        var schema = requestExecutor.Schema.Print();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();
    }

    [Fact]
    public async Task AddApiGraphQLServer_And_MapApiGraphQL_In_Dev_Mode_Should_Add_GraphQL_Server_And_Map_Endpoints()
    {
        var isDev = true;
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
        queryResult.RootElement.GetProperty("extensions").GetProperty("tracing").GetRawText().Should().NotBeEmpty();
    }
    [Fact]
    public async Task AddApiGraphQLServer_And_MapApiGraphQL_In_Release_Mode_Should_Add_GraphQL_Server_And_Map_Endpoints()
    {
        var isDev = false;
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
        queryResult.RootElement.TryGetProperty("extensions", out var _).Should().BeFalse();
    }
}
