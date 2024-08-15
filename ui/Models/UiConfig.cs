namespace ui.Models;

[ExcludeFromCodeCoverage]
public record UiConfig
{
    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string Title { get; init; } = default!;

    [StringLength(5, MinimumLength = 1)]
    public string Separator { get; init; } = " - ";
}
