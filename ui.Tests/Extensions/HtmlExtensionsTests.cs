using AngleSharp.Html.Parser;
using ui.Extensions;

namespace ui.Tests.Extensions;

public class HtmlExtensionsTests
{
    private readonly IHtmlParser _parser = HtmlExtensions.ParserFactory();

    [Fact]
    public void AddHtmlParser_Should_Add_Parser()
    {
        var serviceProvider =
            new ServiceCollection()
                .AddHtmlParser()
                .BuildServiceProvider();

        var result = serviceProvider.GetRequiredService<IHtmlParser>();

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("Hello!", "Hello!")]
    public async Task ToParsedPlainText_Should_Return_Parsed_Content_When_Criteria_Is_Met(
        string? content,
        string expected
    )
    {
        var result = await content.ToParsedPlainText(_parser);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, "", "")]
    [InlineData("", "", "")]
    [InlineData("Hello!", "Hello!", "Hello!")]
    [InlineData("Hello!\r\nWorld!", "Hello!<br />World!", "Hello!\r\nWorld!")]
    public async Task ToParsedHtml_Should_Return_Parsed_Content_When_Criteria_Is_Met(
        string? content,
        string expectedHtml,
        string expectedPlainText
    )
    {
        var result = await content.ToParsedHtml(_parser);

        Assert.Equal(expectedHtml, result.Html);
        Assert.Equal(expectedPlainText, result.PlainText);
    }
}
