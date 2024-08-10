using ui.Services;
using System.Runtime.CompilerServices;
using AngleSharp.Html.Parser;

namespace ui.Extensions;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddValidatedOptions<SmtpConfig>();
        services.AddValidatedOptions<EmailConfig>();

        services.AddTransient<ISmtpService, SmtpService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    public static async IAsyncEnumerable<ValidationResult> ValidateEmailContent(
        this string content,
        IHtmlParser parser,
        string memberName,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        using var document = await parser.ParseDocumentAsync(content, cancellationToken);

        string[] memberNames = [memberName];

        if (string.IsNullOrWhiteSpace(content))
        {
            yield return new ValidationResult(nameof(EmailErrorCodeType.EMPTY_CONTENT), memberNames);
            yield break;
        }

        if (string.IsNullOrWhiteSpace(document.Body!.TextContent))
            yield return new ValidationResult(nameof(EmailErrorCodeType.UNUSABLE_CONTENT), memberNames);

        if (document.GetAllElementsWithAttributes().Any())
            yield return new ValidationResult(nameof(EmailErrorCodeType.HTML_ATTRIBUTES_ARE_NOT_ALLOWED), memberNames);

        if (document.GetAllScriptElements().Length != 0)
            yield return new ValidationResult(nameof(EmailErrorCodeType.SCRIPT_TAGS_ARE_NOT_ALLOWED), memberNames);

        if (document.GetAllStyleElements().Length != 0)
            yield return new ValidationResult(nameof(EmailErrorCodeType.STYLE_TAGS_ARE_NOT_ALLOWED), memberNames);

        if (document.GetAllLinkElements().Length != 0)
            yield return new ValidationResult(nameof(EmailErrorCodeType.LINK_TAGS_ARE_NOT_ALLOWED), memberNames);

        if (document.GetAllMetaElements().Length != 0)
            yield return new ValidationResult(nameof(EmailErrorCodeType.META_TAGS_ARE_NOT_ALLOWED), memberNames);
    }
}
