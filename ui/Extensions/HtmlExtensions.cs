using AngleSharp.Html.Parser;

namespace ui.Extensions;

public static class HtmlExtensions
{
    internal static IHtmlParser ParserFactory() => new HtmlParser(new HtmlParserOptions
    {
        IsScripting = false,
        SkipComments = true,
        SkipRCDataText = true,
        SkipCDATA = true,
        SkipScriptText = true
    });

    public static IServiceCollection AddHtmlParser(this IServiceCollection services) =>
        services.AddSingleton(_ => ParserFactory());

    public static async ValueTask<string> ToParsedPlainText(
        this string? content,
        IHtmlParser parser,
        CancellationToken cancellationToken = default
    )
    {
        if (content is not { Length: > 0 }) return string.Empty;

        using var document = await parser.ParseDocumentAsync(content, cancellationToken);

        return document.Body!.TextContent;
    }

    public static async ValueTask<ParsedHtml> ToParsedHtml(
        this string? content,
        IHtmlParser parser,
        CancellationToken cancellationToken = default
    )
    {
        if (content is not { Length: > 0 }) return new(string.Empty, string.Empty);

        using var document = await parser.ParseDocumentAsync(content, cancellationToken);

        var trimmed = document.Body!.TextContent.ReplaceLineEndings("\r\n");
        var html = trimmed.ReplaceLineEndings("<br />");

        return new(html, trimmed);
    }
}
