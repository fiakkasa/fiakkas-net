using AngleSharp.Html.Parser;

namespace ui.Extensions.Tests;

public class HtmlExtensionsTests
{
    private readonly HtmlParser _parser = new(new HtmlParserOptions
    {
        IsScripting = false,
        SkipComments = true,
        SkipRCDataText = true,
        SkipCDATA = true,
        SkipScriptText = true
    });

    [Fact]
    public void AddHtmlParser_Should_Add_Parser()
    {
        var serviceProvider =
            new ServiceCollection()
                .AddHtmlParser()
                .BuildServiceProvider();

        var result = serviceProvider.GetRequiredService<IHtmlParser>();

        result.Should().NotBeNull();
    }

    [Fact]
    public void GetAllElementsWithAttributes_Should_Return_Elements_With_Attributes_When_Present()
    {
        var html = "<div class='container'><p>Hello World!</p></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllElementsWithAttributes();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAllElementsWithAttributes_Should_Return_No_Elements_With_Attributes_When_Not_Present()
    {
        var html = "<div><p>Hello World!</p></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllElementsWithAttributes();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllScriptElements_Should_Return_Script_Elements_When_Present()
    {
        var html = "<div><script></script></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllScriptElements();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAllScriptElements_Should_Not_Return_Script_Elements_When_Not_Present()
    {
        var html = "<div></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllScriptElements();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllStyleElements_Should_Return_Script_Elements_When_Present()
    {
        var html = "<div><style></style></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllStyleElements();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAllStyleElements_Should_Not_Return_Script_Elements_When_Not_Present()
    {
        var html = "<div></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllStyleElements();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllLinkElements_Should_Return_Script_Elements_When_Present()
    {
        var html = "<div><link /></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllLinkElements();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAllLinkElements_Should_Not_Return_Script_Elements_When_Not_Present()
    {
        var html = "<div></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllLinkElements();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllMetaElements_Should_Return_Script_Elements_When_Present()
    {
        var html = "<div><meta /></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllMetaElements();

        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAllMetaElements_Should_Not_Return_Script_Elements_When_Not_Present()
    {
        var html = "<div></div>";
        var document = _parser.ParseDocument(html);

        var result = document.GetAllMetaElements();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ToParsedPlainText_Should_Return_Parsed_Content_When_Content_Is_Parsable()
    {
        var html = "<div>Hello!</div>";

        var result = await html.ToParsedPlainText(_parser);

        result.Should().Be("Hello!");
    }

    [Fact]
    public async Task ToParsedPlainText_Should_Return_Empty_Parsed_Content_When_Content_Is_Null()
    {
        string? html = default;

        var result = await html.ToParsedPlainText(_parser);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ToParsedPlainText_Should_Return_Empty_Parsed_Content_When_Content_Is_Not_Parsable()
    {
        string html = "<div <span ";

        var result = await html.ToParsedPlainText(_parser);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ToParsedHtml_Should_Return_Parsed_Content_When_Content_Is_Parsable()
    {
        var html = "<div>Hello!</div>";

        var result = await html.ToParsedHtml(_parser);

        result.Html.Should().Be(html);
        result.PlainText.Should().Be("Hello!");
    }

    [Fact]
    public async Task ToParsedHtml_Should_Return_Empty_Parsed_Content_When_Content_Is_Null()
    {
        string? html = default;

        var result = await html.ToParsedHtml(_parser);

        result.Html.Should().BeEmpty();
        result.PlainText.Should().BeEmpty();
    }

    [Fact]
    public async Task ToParsedHtml_Should_Return_Empty_Parsed_Content_When_Content_Is_Not_Parsable()
    {
        string html = "<div <span ";

        var result = await html.ToParsedHtml(_parser);

        result.Html.Should().BeEmpty();
        result.PlainText.Should().BeEmpty();
    }
}
