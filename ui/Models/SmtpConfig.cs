namespace ui.Models;

[ExcludeFromCodeCoverage]
public record SmtpConfig
{
    [Required]
    public string Host { get; set; } = default!;

    [Range(25, 65_536)]
    public int Port { get; set; } = 25;

    [StringLength(128, MinimumLength = 1)]
    public string? Username { get; set; }

    [StringLength(512, MinimumLength = 1)]
    public string? Password { get; set; }

    public bool EnableSsl { get; set; }
}
