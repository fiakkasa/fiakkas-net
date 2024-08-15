namespace ui.Models;

[ExcludeFromCodeCoverage]
public record FiakkasNetApiConfig
{
    [Required]
    public Uri BaseUrl { get; init; } = default!;
}
