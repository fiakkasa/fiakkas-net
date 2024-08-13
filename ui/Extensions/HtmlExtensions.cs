using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace ui.Extensions;

public static class HtmlExtensions
{
    public static IServiceCollection AddHtmlParser(this IServiceCollection services) =>
        services.AddSingleton<IHtmlParser>(new HtmlParser(new HtmlParserOptions
        {
            IsScripting = false,
            SkipComments = true,
            SkipRCDataText = true,
            SkipCDATA = true,
            SkipScriptText = true
        }));

    public static IEnumerable<IElement> GetAllElementsWithAttributes(this IParentNode node) =>
        node.QuerySelectorAll("*").Where(x => x.Attributes.Length != 0);

    public static IHtmlCollection<IElement> GetAllScriptElements(this IParentNode node) =>
        node.QuerySelectorAll("script");

    public static IHtmlCollection<IElement> GetAllStyleElements(this IParentNode node) =>
        node.QuerySelectorAll("style");

    public static IHtmlCollection<IElement> GetAllLinkElements(this IParentNode node) =>
        node.QuerySelectorAll("link");

    public static IHtmlCollection<IElement> GetAllMetaElements(this IParentNode node) =>
        node.QuerySelectorAll("meta");

    public static async ValueTask<string> ToParsedPlainText(
        this string? content,
        IHtmlParser parser,
        CancellationToken cancellationToken = default
    )
    {
        using var document = await parser.ParseDocumentAsync(content ?? string.Empty, cancellationToken);

        return document.Body!.TextContent.Trim();
    }

    public static async ValueTask<ParsedHtml> ToParsedHtml(
        this string? content,
        IHtmlParser parser,
        CancellationToken cancellationToken = default
    )
    {
        using var document = await parser.ParseDocumentAsync(content ?? string.Empty, cancellationToken);

        return new(
            document.Body!.InnerHtml,
            document.Body.TextContent.Trim()
        );
    }
}
