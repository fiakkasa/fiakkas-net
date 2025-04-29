namespace ui.Models;

[ExcludeFromCodeCoverage]
public record UiConfig
{
    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [StringLength(5, MinimumLength = 1)]
    public string Separator { get; init; } = " - ";

    [Required]
    [StringLength(1024, MinimumLength = 1)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [StringLength(1024, MinimumLength = 1)]
    public string Keywords { get; init; } = string.Empty;

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string Author { get; init; } = string.Empty;

    [Required]
    [Range(0, 60_000)]
    public int FullScreenLoaderTransitionDelay { get; init; }

    [Required]
    [Range(0, 60_000)]
    public int FullScreenLoaderTransitionDuration { get; init; }
}
