using System.Net;
using System.Net.Mail;

namespace ui.Services;

[ExcludeFromCodeCoverage]
public class SmtpService(IOptionsSnapshot<SmtpConfig> optionsSnapshot) : ISmtpService
{
    private SmtpClient GetClient() => new(optionsSnapshot.Value.Host)
    {
        Port = optionsSnapshot.Value.Port,
        EnableSsl = optionsSnapshot.Value.EnableSsl,
        UseDefaultCredentials = optionsSnapshot.Value is not { Username.Length: > 0, Password.Length: > 0 },
        Credentials = optionsSnapshot.Value switch
        {
            { Username: { Length: > 0 } username, Password: { Length: > 0 } password } =>
                new NetworkCredential(username, password),
            _ => default
        }
    };

    public async ValueTask Send(MailMessage message, CancellationToken cancellationToken = default)
    {
        using var client = GetClient();

        await client.SendMailAsync(message, cancellationToken);
    }
}
