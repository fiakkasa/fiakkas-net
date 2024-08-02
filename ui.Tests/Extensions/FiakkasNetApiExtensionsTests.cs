using System.Net.Http;
using ui.GraphQL;
using ui.Models;

namespace ui.Extensions.Tests;

public class FiakkasNetApiExtensionsTests
{
    [Fact]
    public void FiakkasNetApiExtensions_Should_Add_Client_With_Options()
    {
        var expected = new FiakkasNetApiConfig
        {
            BaseUrl = new Uri("https://test.com")
        };
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(
                    new Dictionary<string, object>
                    {
                        [nameof(FiakkasNetApiConfig)] = expected
                    }.ToConfiguration()
                )
                .AddFiakkasNetApiClient()
                .BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptionsSnapshot<FiakkasNetApiConfig>>();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory?.CreateClient(FiakkasNetApi.ClientName);

        clientFactory.Should().NotBeNull();
        client.Should().NotBeNull();
        client!.BaseAddress.Should().Be(expected.BaseUrl);
        options.Value.Should().Be(expected);
    }
}
