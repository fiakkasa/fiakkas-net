namespace ui.Models;

[ExcludeFromCodeCoverage]
public record EmailConfig
{
    public bool AlwaysUseDefaultSenderAddress { get; set; }

    [EmailAddress]
    public string DefaultSenderAddress { get; set; } = string.Empty;

    [EmailAddress]
    public string DefaultRecipientAddress { get; set; } = string.Empty;

    [StringLength(128, MinimumLength = 1)]
    public string DefaultPlainTextSignature { get; set; } = string.Empty;

    [StringLength(256, MinimumLength = 1)]
    public string DefaultHtmlSignature { get; set; } = string.Empty;
}
