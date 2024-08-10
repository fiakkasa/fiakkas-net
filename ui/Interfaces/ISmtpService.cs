using System.Net.Mail;

namespace ui.Interfaces;

public interface ISmtpService
{
    ValueTask Send(MailMessage message, CancellationToken cancellationToken = default);
}
