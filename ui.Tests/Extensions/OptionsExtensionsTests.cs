using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using ui.Extensions;

namespace ui.Tests.Extensions;

public class OptionsExtensionsTests
{
    [Fact]
    public async Task AddValidatedOptions_Should_Add_Validated_Options_With_Section_Key_And_Default_Key()
    {
        using var host = await new HostBuilder()
            .ConfigureHostConfiguration(builder =>
                builder.AddToConfigurationBuilder(new Dictionary<string, object>
                {
                    [nameof(MockConfig)] = new MockConfig
                    {
                        Name = "Default"
                    },
                    ["SectionKey"] = new MockConfigAlt
                    {
                        Name = "Alt"
                    }
                })
            )
            .ConfigureWebHost(webBuilder =>
                webBuilder
                    .UseTestServer()
                    .ConfigureServices((_, services) =>
                    {
                        services.AddValidatedOptions<MockConfig>();
                        services.AddValidatedOptions<MockConfigAlt>("SectionKey");
                    })
                    .Configure(_ => { })
            )
            .StartAsync();

        var services = host.GetTestServer().Services;

        var results = services.GetServices<IOptionsSnapshot<MockConfig>>().ToArray();
        var resultsAlt = services.GetServices<IOptionsSnapshot<MockConfigAlt>>().ToArray();

        results.Should().ContainSingle();
        resultsAlt.Should().ContainSingle();
        results.First().Value.Name.Should().Be("Default");
        resultsAlt.First().Value.Name.Should().Be("Alt");
    }

    public record MockConfig
    {
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }

    public record MockConfigAlt
    {
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }
}
