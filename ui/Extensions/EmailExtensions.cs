using AngleSharp.Html.Dom;
using System.Net.Mail;
using ui.Services;

namespace ui.Extensions;

public static class EmailExtensions
{
    private static readonly Dictionary<string, ValidationAttribute[]> _emailValidationItems = new()
    {
        [nameof(EmailErrorCodeType.REQUIRED)] = [new RequiredAttribute()],
        [nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS)] = [new EmailAddressAttribute()]
    };

    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddValidatedOptions<SmtpConfig>();
        services.AddValidatedOptions<EmailConfig>();

        services.AddTransient<ISmtpService, SmtpService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    public static IEnumerable<ValidationResult> ValidateEmailAddress(this string emailAddress, string memberName)
    {
        string[] memberNames = [memberName];
        var validationContext = new ValidationContext(emailAddress) { MemberName = memberName };

        foreach (var (Key, Value) in _emailValidationItems)
        {
            if (!Validator.TryValidateValue(emailAddress, validationContext, default, Value))
                yield return new(Key, memberNames);
        }
    }

    public static IEnumerable<ValidationResult> ValidateEmailContent(this IHtmlDocument document, string memberName)
    {
        string[] memberNames = [memberName];

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

    public static MailAddress GetSenderMailAddress(
        this string senderAddress,
        bool useDefaultSenderAddress,
        string defaultSenderAddress
    ) =>
        new(
            useDefaultSenderAddress switch
            {
                true => defaultSenderAddress,
                _ => senderAddress
            }
        );

    public static string GetSubject(this string subject, bool isBehalfOf, string senderAddress) =>
        isBehalfOf switch
        {
            true => $"On Behalf of <{senderAddress}> | {subject}",
            _ => subject
        };
}
