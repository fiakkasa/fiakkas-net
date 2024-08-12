namespace ui.Models;

[ExcludeFromCodeCoverage]
public record EmailConfig
{
    public bool AlwaysUseDefaultSenderAddress { get; set; }

    [EmailAddress]
    public string DefaultSenderAddress { get; set; } = string.Empty;

    [EmailAddress]
    public string DefaultRecipientAddress { get; set; } = string.Empty;

    [StringLength(128)]
    public string PlainTextSignature { get; set; } = string.Empty;

    [StringLength(256)]
    public string HtmlSignature { get; set; } = string.Empty;
}
