using AngleSharp.Html.Parser;
using System.Net.Mail;
using System.Text;
using ui.Extensions;

namespace ui.Services;

public class EmailService(
    ISmtpService smtp,
    IHtmlParser parser,
    IOptionsSnapshot<EmailConfig> optionsSnapshot,
    ILogger<EmailService> logger
) : IEmailService
{
    private static IEnumerable<ValidationResult> ValidateEmailRawContent(string content, string memberName)
    {
        if (string.IsNullOrWhiteSpace(content))
            yield return new(nameof(EmailErrorCodeType.EMPTY_CONTENT), [memberName]);
    }

    private async ValueTask<IReadOnlyCollection<ValidationResult>> Validate(
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        var validationResults = new List<ValidationResult>();

        validationResults.AddRange(senderAddress.ValidateEmailAddress(EmailConsts.SenderAddressFieldName));
        validationResults.AddRange(recipientAddress.ValidateEmailAddress(EmailConsts.RecipientAddressFieldName));

        var subjectEmptyValidationResults = ValidateEmailRawContent(subject, EmailConsts.SubjectFieldName).ToArray();
        if (subjectEmptyValidationResults.Length > 0)
        {
            validationResults.AddRange(subjectEmptyValidationResults);
        }
        else
        {
            using var subjectDocument = await parser.ParseDocumentAsync(subject, cancellationToken);
            validationResults.AddRange(subjectDocument.ValidateEmailPlainTextContent(EmailConsts.SubjectFieldName));
        }

        var bodyEmptyValidationResults = ValidateEmailRawContent(body, EmailConsts.BodyFieldName).ToArray();
        if (bodyEmptyValidationResults.Length > 0)
        {
            validationResults.AddRange(ValidateEmailRawContent(body, EmailConsts.BodyFieldName));
        }
        else
        {
            using var bodyDocument = await parser.ParseDocumentAsync(body, cancellationToken);
            validationResults.AddRange(bodyDocument.ValidateEmailHtmlContent(EmailConsts.BodyFieldName));
        }

        return validationResults;
    }

    public async ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> Send(
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var emailConfig = optionsSnapshot.Value;

            var validationResults = new List<ValidationResult>(
                await Validate(senderAddress, recipientAddress, subject, body, cancellationToken)
            );

            if (validationResults.Count > 0)
                return validationResults;

            var message = await BuildMessage(emailConfig, senderAddress, recipientAddress, subject, body, cancellationToken);

            await smtp.Send(message, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email from {SenderAddress} to {RecipientAddress}", senderAddress, recipientAddress);

            return new InvalidOperationException(nameof(EmailErrorCodeType.FAILED_TO_SEND), ex);
        }
    }

    private async ValueTask<MailMessage> BuildMessage(
        EmailConfig emailConfig,
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        var parsedSubject = await subject.ToParsedPlainText(parser, cancellationToken);
        var parsedBody = await body.ToParsedHtml(parser, cancellationToken);
        var message = new MailMessage
        {
            From = senderAddress.GetSenderMailAddress(emailConfig.AlwaysUseDefaultSenderAddress, emailConfig.DefaultSenderAddress),
            Subject = parsedSubject.GetSubject(emailConfig.AlwaysUseDefaultSenderAddress, senderAddress),
            SubjectEncoding = Encoding.UTF8,
            Body = parsedBody.Html + emailConfig.HtmlSignature,
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = true
        };
        message.To.Add(recipientAddress);

        message.AlternateViews.Add(
            AlternateView.CreateAlternateViewFromString(
                parsedBody.PlainText + emailConfig.PlainTextSignature,
                Encoding.UTF8,
                "text/plain"
            )
        );

        return message;
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
            return await Send(senderAddress, optionsSnapshot.Value.DefaultRecipientAddress, subject, body, cancellationToken);
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
            return await Send(optionsSnapshot.Value.DefaultSenderAddress, recipientAddress, subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email from {SenderAddress} to {RecipientAddress}", nameof(EmailConfig.DefaultSenderAddress), recipientAddress);

            return new InvalidOperationException(nameof(EmailErrorCodeType.FAILED_TO_SEND), ex);
        }
    }
}
