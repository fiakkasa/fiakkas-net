namespace ui.Interfaces;

public interface IEmailService
{
    ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> Send(
        string senderAddress,
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    );
    ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> SendFrom(
        string senderAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    );
    ValueTask<OneOf<bool, IReadOnlyCollection<ValidationResult>, InvalidOperationException>> SendTo(
        string recipientAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    );
}
