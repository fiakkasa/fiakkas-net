using AngleSharp.Html.Parser;
using System.Net.Mail;
using System.Text;
using ui.Extensions;

namespace ui.Services;

public class EmailService(
    IOptionsSnapshot<EmailConfig> optionsSnapshot,
    ISmtpService smtp,
    IHtmlParser parser,
    ILogger<EmailService> logger
) : IEmailService
{
    private static async ValueTask<IReadOnlyCollection<ValidationResult>> Validate(
        IHtmlParser parser,
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        var validationResults = new List<ValidationResult>();

        validationResults.AddRange(ValidateEmailAddress(senderAddress, EmailConsts.SenderAddressFieldName));
        validationResults.AddRange(ValidateEmailAddress(recipientAddress, EmailConsts.RecipientAddressFieldName));
        validationResults.AddRange(await ValidateEmailContent(subject, parser, EmailConsts.SubjectFieldName, cancellationToken));
        validationResults.AddRange(await ValidateEmailContent(body, parser, EmailConsts.BodyFieldName, cancellationToken));

        return validationResults;

        static IReadOnlyCollection<ValidationResult> ValidateEmailAddress(string senderAddress, string memberName)
        {
            string[] memberNames = [memberName];
            var validationContext = new ValidationContext(senderAddress) { MemberName = memberName };

            if (!Validator.TryValidateValue(senderAddress, validationContext, [], [new RequiredAttribute()]))
                return [new(nameof(EmailErrorCodeType.REQUIRED), memberNames)];

            if (!Validator.TryValidateValue(senderAddress, validationContext, [], [new EmailAddressAttribute()]))
                return [new(nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS), memberNames)];

            return [];
        }

        static async ValueTask<IReadOnlyCollection<ValidationResult>> ValidateEmailContent(
            string content,
            IHtmlParser parser,
            string memberName,
            CancellationToken cancellationToken
        )
        {
            var contentValidationResults = new List<ValidationResult>();

            await foreach (var item in content.ValidateEmailContent(parser, memberName, cancellationToken))
                contentValidationResults.Add(item);

            return contentValidationResults;
        }
    }

    public async ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> SendFromTo(
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var defaultHtmlSignature = optionsSnapshot.Value.DefaultHtmlSignature;
            var defaultPlainTextSignature = optionsSnapshot.Value.DefaultPlainTextSignature;

            var validationResults = new List<ValidationResult>(
                await Validate(parser, senderAddress, recipientAddress, subject, body, cancellationToken)
            );

            if (validationResults.Count > 0)
                return validationResults;

            var parsedSubject = await subject.ToParsedHtml(parser, cancellationToken);
            var parsedBody = await body.ToParsedHtml(parser, cancellationToken);
            var message = new MailMessage
            {
                From = new MailAddress(senderAddress),
                Subject = parsedSubject.PlainText,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            message.To.Add(recipientAddress);

            message.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(
                    parsedBody.Html + defaultHtmlSignature,
                    Encoding.UTF8,
                    "text/html"
                )
            );
            message.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(
                    parsedBody.PlainText + defaultPlainTextSignature,
                    Encoding.UTF8,
                    "text/plain"
                )
            );

            //await smtp.Send(message, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email from {SenderAddress} to {RecipientAddress}", senderAddress, recipientAddress);

            return new InvalidOperationException(nameof(EmailErrorCodeType.FAILED_TO_SEND), ex);
        }
    }

    public async ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> SendFrom(
        string senderAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await SendFromTo(senderAddress, optionsSnapshot.Value.DefaultRecipientAddress, subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email from {SenderAddress} to {RecipientAddress}", senderAddress, nameof(EmailConfig.DefaultRecipientAddress));

            return new InvalidOperationException(nameof(EmailErrorCodeType.FAILED_TO_SEND), ex);
        }
    }

    public async ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> SendTo(string recipientAddress, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            return await SendFromTo(optionsSnapshot.Value.DefaultSenderAddress, recipientAddress, subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email from {SenderAddress} to {RecipientAddress}", nameof(EmailConfig.DefaultSenderAddress), recipientAddress);

            return new InvalidOperationException(nameof(EmailErrorCodeType.FAILED_TO_SEND), ex);
        }
    }
}
