using System.ComponentModel.DataAnnotations;

namespace ui.Models;

public record UiConfig
{
    [Required]
    public string Title { get; set; } = default!;

    public string Separator { get; set; } = " - ";
}
