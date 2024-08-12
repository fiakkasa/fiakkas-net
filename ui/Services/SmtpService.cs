using System.Net.Mail;
using ui.Extensions;

namespace ui.Services;

[ExcludeFromCodeCoverage]
public class SmtpService(IOptionsSnapshot<SmtpConfig> optionsSnapshot) : ISmtpService
{
    public async ValueTask Send(MailMessage message, CancellationToken cancellationToken = default)
    {
        using var client = optionsSnapshot.Value.GetSmtpClient();

        await client.SendMailAsync(message, cancellationToken);
    }
}
