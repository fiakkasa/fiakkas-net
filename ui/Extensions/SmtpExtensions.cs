using System.Net;
using System.Net.Mail;

namespace ui.Extensions;

public static class SmtpExtensions
{
    public static bool UseDefaultCredentials(this SmtpConfig config) =>
        config is not { Username.Length: > 0, Password.Length: > 0 };

    public static NetworkCredential? GetNetworkCredentials(this SmtpConfig config) =>
        config switch
        {
            { Username: { Length: > 0 } username, Password: { Length: > 0 } password } =>
                new(username, password),
            _ => default
        };

    public static SmtpClient GetSmtpClient(this SmtpConfig config) =>
        new(config.Host)
        {
            Port = config.Port,
            EnableSsl = config.EnableSsl,
            UseDefaultCredentials = config.UseDefaultCredentials(),
            Credentials = config.GetNetworkCredentials()
        };
}
