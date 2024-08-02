using ui.Models;

namespace ui.Extensions.Tests;

public class UiExtensionsTests
{
    [Fact]
    public void AddUiConfig_Should_Add_ValidatedOptions()
    {
        var expected = new UiConfig
        {
            Title = "Title",
            Separator = " - "
        };
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(
                    new Dictionary<string, object>
                    {
                        [nameof(UiConfig)] = expected
                    }.ToConfiguration()
                )
                .AddUiConfig()
                .BuildServiceProvider();

        var result = serviceProvider.GetRequiredService<IOptionsSnapshot<UiConfig>>();

        result.Value.Should().Be(expected);
    }
}
