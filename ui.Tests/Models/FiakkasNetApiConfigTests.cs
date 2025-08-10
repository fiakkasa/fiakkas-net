using Polly;
using ui.Models;

namespace ui.Tests.Models;

public class FiakkasNetApiConfigTests
{
    [Fact]
    public void FiakkasNetApiConfig_Should_Validate_Successfully()
    {
        var config = new FiakkasNetApiConfig
        {
            BaseUrl = new Uri("https://test.com"),
            DelayBackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromMilliseconds(400)
        };

        var result = Validator.TryValidateObject(config, new(config), null, true);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1_000_000)]
    public void FiakkasNetApiConfig_Should_Not_Validate_Successfully_On_Invalid_Delay(int delayMilliseconds)

    {
        var config = new FiakkasNetApiConfig
        {
            BaseUrl = new Uri("https://test.com"),
            DelayBackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromMilliseconds(delayMilliseconds)
        };

        var result = Validator.TryValidateObject(config, new(config), null, true);

        Assert.False(result);
    }
}
