using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace api.Extensions.Tests;

public class CorsExtensionsTests
{
    private static HttpRequestMessage GetRequestMessage(HttpMethod method)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Options,
            RequestUri = new("/hello", UriKind.Relative)
        };
        request.Headers.Add("Access-Control-Request-Method", method.ToString());
        request.Headers.Add("Access-Control-Request-Headers", "*");
        request.Headers.Add("Origin", "http://example.com");

        return request;
    }

    [Fact]
    public async Task UseApiCors_Should_Register_And_Enable_Cors()
    {
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
                services
                    .AddRouting()
                    .AddCors()
            )
            .Configure(app =>
            {
                app.UseRouting();
                app.UseApiCors();
                app.UseEndpoints(endpoints =>
                    endpoints.Map("/hello", () => "Hello!").RequireCors()
                );
            });

        using var server = new TestServer(builder);
        var client = server.CreateClient();
        var service = server.Services.GetService<ICorsService>();

        var results = await Task.WhenAll(
            client.SendAsync(GetRequestMessage(HttpMethod.Options)),
            client.SendAsync(GetRequestMessage(HttpMethod.Post)),
            client.SendAsync(GetRequestMessage(HttpMethod.Put))
        );

        service.Should().NotBeNull();
        results.Should().AllSatisfy(x =>
        {
            x.StatusCode.Should().Be(HttpStatusCode.NoContent);
            x.Headers.Should().ContainSingle(h => h.Key == "Access-Control-Allow-Origin" && h.Value.First() == "*");
            x.Headers.Should().ContainSingle(h => h.Key == "Access-Control-Allow-Headers" && h.Value.First() == "*");
            x.Headers.Should().ContainSingle(h => h.Key == "Access-Control-Allow-Methods" && h.Value.First() == HttpMethods.Post);
        });
    }
}
