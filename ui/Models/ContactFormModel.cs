namespace ui.Models;

[ExcludeFromCodeCoverage]
public record ContactFormModel
{
    [Required(ErrorMessage = "Please enter your email address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string FromEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a subject")]
    [StringLength(100, MinimumLength = 1)]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a message")]
    [StringLength(1000, MinimumLength = 1)]
    public string Message { get; set; } = string.Empty;
}
