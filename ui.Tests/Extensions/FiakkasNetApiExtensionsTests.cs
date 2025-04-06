using Snapshooter.Xunit;
using System.Net.Http;
using ui.Extensions;
using ui.GraphQL;
using ui.Models;

namespace ui.Tests.Extensions;

public class FiakkasNetApiExtensionsTests
{
    [Fact]
    public void FiakkasNetApiExtensions_Should_Add_Client_With_Options()
    {
        var expected = new FiakkasNetApiConfig
        {
            BaseUrl = new("https://test.com")
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

        Assert.NotNull(clientFactory);
        Assert.NotNull(client);
        Assert.Equivalent(expected.BaseUrl, client.BaseAddress, true);
        Assert.Equivalent(expected, options.Value, true);

        options.MatchSnapshot();
    }
}
