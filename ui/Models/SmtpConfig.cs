namespace ui.Models;

[ExcludeFromCodeCoverage]
public record SmtpConfig
{
    [Required]
    public string Host { get; init; } = default!;

    [Range(25, 65_536)]
    public int Port { get; init; } = 25;

    [StringLength(128, MinimumLength = 1)]
    public string? Username { get; init; }

    [StringLength(512, MinimumLength = 1)]
    public string? Password { get; init; }

    public bool EnableSsl { get; init; }
}
