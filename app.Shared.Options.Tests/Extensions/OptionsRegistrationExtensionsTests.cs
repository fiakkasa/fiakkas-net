namespace app.Shared.Options.Tests.Extensions;

public class OptionsRegistrationExtensionsTests
{
    [Theory]
    [InlineData("test")]
    [InlineData("  test  ")]
    public void Adding_Bound_Options_Should_Register_In_Dependency_Injection_Using_Section_Name(string sectionPath)
    {
        var config = new Dictionary<string, object>
        {
            ["test"] = new
            {
                Name = "Hello"
            }
        }.ToConfiguration();

        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(config)
                .AddBoundOptions<MockBoundConfig>(sectionPath)
                .Services
                .BuildServiceProvider();

        var result = serviceProvider.GetService<IOptions<MockBoundConfig>>();

        result.Should().NotBeNull();
        result!.Value.Name.Should().Be("Hello");
    }

    [Fact]
    public void
        Adding_Bound_Options_Should_Register_In_Dependency_Injection_Using_Class_Nam_When_No_Section_Path_Is_Provided()
    {
        var config = new Dictionary<string, object>
        {
            [nameof(MockBoundConfig)] = new
            {
                Name = "Hello"
            }
        }.ToConfiguration();

        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(config)
                .AddBoundOptions<MockBoundConfig>()
                .Services
                .BuildServiceProvider();

        var result = serviceProvider.GetService<IOptions<MockBoundConfig>>();

        result.Should().NotBeNull();
        result!.Value.Name.Should().Be("Hello");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void
        Adding_Bound_Options_Should_Register_In_Dependency_Injection_Using_Class_Nam_When_Section_Path_Evaluates_To_Empty(
            string? sectionPath
        )
    {
        var config = new Dictionary<string, object>
        {
            [nameof(MockBoundConfig)] = new
            {
                Name = "Hello"
            }
        }.ToConfiguration();

        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(config)
                .AddBoundOptions<MockBoundConfig>(sectionPath)
                .Services
                .BuildServiceProvider();

        var result = serviceProvider.GetService<IOptions<MockBoundConfig>>();

        result.Should().NotBeNull();
        result!.Value.Name.Should().Be("Hello");
    }

    [Fact]
    public async Task AddValidatedOptions_Should_Add_Validated_Options_With_Section_Key_And_Default_Key()
    {
        using var host = await new HostBuilder()
            .ConfigureHostConfiguration(builder =>
                builder.AddToConfigurationBuilder(new Dictionary<string, object>
                {
                    [nameof(MockValidatedConfig)] = new MockValidatedConfig
                    {
                        Name = "Default"
                    },
                    ["SectionKey"] = new MockValidatedConfigAlt
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
                        services.AddValidatedOptions<MockValidatedConfig>();
                        services.AddValidatedOptions<MockValidatedConfigAlt>("SectionKey");
                    })
                    .Configure(_ => { })
            )
            .StartAsync();

        var services = host.GetTestServer().Services;

        var results = services.GetServices<IOptionsSnapshot<MockValidatedConfig>>().ToArray();
        var resultsAlt = services.GetServices<IOptionsSnapshot<MockValidatedConfigAlt>>().ToArray();

        results.Should().ContainSingle();
        resultsAlt.Should().ContainSingle();
        results.First().Value.Name.Should().Be("Default");
        resultsAlt.First().Value.Name.Should().Be("Alt");
    }

    public record MockBoundConfig
    {
        public string? Name { get; set; }
    }

    public record MockValidatedConfig
    {
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }

    public record MockValidatedConfigAlt
    {
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }
}
