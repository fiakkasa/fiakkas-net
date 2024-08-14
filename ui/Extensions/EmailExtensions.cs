using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net.Mail;
using ui.Services;

namespace ui.Extensions;

public static class EmailExtensions
{
    private static readonly Dictionary<string, ValidationAttribute[]> EmailValidationItems = new()
    {
        [nameof(EmailErrorCodeType.Required)] = [new RequiredAttribute()],
        [nameof(EmailErrorCodeType.InvalidEmailAddress)] = [new EmailAddressAttribute()]
    };

    private static readonly Dictionary<string, ValidationAttribute[]> EmailContentValidationItems = new()
    {
        [nameof(EmailErrorCodeType.Required)] = [new RequiredAttribute()]
    };

    private static readonly Dictionary<string, Func<IHtmlDocument, bool>> EmailDocumentValidationItems = new()
    {
        [nameof(EmailErrorCodeType.MarkupIsNotAllowed)] =
            document => document is { Head.Children.Length: 0, Body.Children.Length: 0 },
        [nameof(EmailErrorCodeType.UnusableContent)] = document => document.Body!.TextContent.Length > 0
    };

    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddValidatedOptions<SmtpConfig>();
        services.AddValidatedOptions<EmailConfig>();

        services.AddTransient<ISmtpService, SmtpService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    public static IReadOnlyCollection<ValidationResult> ValidateEmailAddress(
        this string? emailAddress,
        string memberName
    )
    {
        string[] memberNames = [memberName];
        var normalizedEmailAddress = emailAddress ?? string.Empty;
        var validationContext = new ValidationContext(normalizedEmailAddress) { MemberName = memberName };

        foreach (var (key, value) in EmailValidationItems)
        {
            if (!Validator.TryValidateValue(normalizedEmailAddress, validationContext, default, value))
            {
                return [new(key, memberNames)];
            }
        }

        return [];
    }

    public static async ValueTask<IReadOnlyCollection<ValidationResult>> ValidateEmailContent(
        this string? content,
        IHtmlParser parser,
        string memberName,
        CancellationToken cancellationToken = default
    )
    {
        string[] memberNames = [memberName];
        var normalizedContent = content ?? string.Empty;
        var validationContext = new ValidationContext(normalizedContent) { MemberName = memberName };

        foreach (var (key, value) in EmailContentValidationItems)
        {
            if (!Validator.TryValidateValue(normalizedContent, validationContext, default, value))
                return [new(key, memberNames)];
        }

        using var document = await parser.ParseDocumentAsync(normalizedContent, cancellationToken);

        foreach (var (key, value) in EmailDocumentValidationItems)
        {
            if (!value(document))
            {
                return [new(key, memberNames)];
            }
        }

        return [];
    }

    public static MailAddress GetSenderMailAddress(
        this string senderAddress,
        bool useDefaultSenderAddress,
        string defaultSenderAddress
    ) => new(
        useDefaultSenderAddress switch
        {
            true => defaultSenderAddress,
            _ => senderAddress
        }
    );

    public static string GetSubject(
        this string subject,
        bool isBehalfOf,
        string senderAddress
    ) => isBehalfOf switch
    {
        true => $"On Behalf of <{senderAddress}> | {subject}",
        _ => subject
    };
}
