using Microsoft.Extensions.Configuration;

namespace app.Shared.Options.Tests.Extensions;

public class IConfigurationExtensionsTests
{
    [Fact]
    public void GetConfiguration_Should_Throw_ValidationException_When_Config_Section_Cannot_Be_Materialized()
    {
        var configuration = new ConfigurationBuilder().Build();

        Assert.Throws<ValidationException>(() => configuration.GetConfiguration<MockConfig>());

        try
        {
            configuration.GetConfiguration<MockConfig>("Test:Hello:World");
        }
        catch (ValidationException ex)
        {
            Assert.Equal(
                "Configuration for type 'MockConfig' at section 'Test:Hello:World' cannot be materialized.",
                ex.Message
            );
        }
    }

    [Fact]
    public void GetConfiguration_Should_Throw_ValidationException_When_Config_Is_Invalid()
    {
        var configuration = new ConfigurationBuilder()
            .AddToConfigurationBuilder(
                new Dictionary<string, object>
                {
                    [nameof(MockConfig)] = new MockConfig
                    {
                        Count = 100
                    },
                    ["Test"] = new
                    {
                        Hello = new
                        {
                            World = new MockConfig
                            {
                                Count = 100
                            }
                        }
                    }
                }
            )
            .Build();

        Assert.Throws<ValidationException>(() => configuration.GetConfiguration<MockConfig>());

        try
        {
            configuration.GetConfiguration<MockConfig>("Test:Hello:World");
        }
        catch (ValidationException ex)
        {
            Assert.Equal(
                "Validation failed for type 'MockConfig', at section 'Test:Hello:World', and member(s) 'Count' with message: 'The field Count must be between 1 and 3.'",
                ex.Message
            );
        }
    }

    [Fact]
    public void GetConfiguration_Should_Return_Config_When_Config_Is_Valid()
    {
        var configuration = new ConfigurationBuilder()
            .AddToConfigurationBuilder(
                new Dictionary<string, object>
                {
                    [nameof(MockConfig)] = new MockConfig
                    {
                        Count = 2
                    },
                    ["Test"] = new
                    {
                        Hello = new
                        {
                            World = new MockConfig
                            {
                                Count = 2
                            }
                        }
                    }
                }
            )
            .Build();

        Assert.Equal(2, configuration.GetConfiguration<MockConfig>().Count);
        Assert.Equal(2, configuration.GetConfiguration<MockConfig>("Test:Hello:World").Count);
    }

    public record MockConfig
    {
        [Range(1, 3)]
        public int Count { get; set; }
    }
}
