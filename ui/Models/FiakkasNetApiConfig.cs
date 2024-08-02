namespace ui.Models;

public record FiakkasNetApiConfig
{
    [Required]
    public Uri BaseUrl { get; set; } = default!;
}
