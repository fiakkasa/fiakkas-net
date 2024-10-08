using ui.Extensions;
using ui.Models;

namespace ui.Tests.Extensions;

public class UiExtensionsTests
{
    [Fact]
    public void AddUiConfig_Should_Add_Validated_Options()
    {
        var expected = new UiConfig
        {
            Title = "Title",
            Separator = " - ",
            Description = "Description",
            Keywords = "Keywords",
            Author = "Author"
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
