namespace ui.Models;

[ExcludeFromCodeCoverage]
public record EmailConfig
{
    public bool AlwaysUseDefaultSenderAddress { get; init; }

    [EmailAddress]
    public string DefaultSenderAddress { get; init; } = string.Empty;

    [EmailAddress]
    public string DefaultRecipientAddress { get; init; } = string.Empty;

    [StringLength(128)]
    public string PlainTextSignature { get; init; } = string.Empty;

    [StringLength(256)]
    public string HtmlSignature { get; init; } = string.Empty;
}
