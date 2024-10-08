namespace ui.Models;

[ExcludeFromCodeCoverage]
public record ContactFormModel
{
    [Required(ErrorMessage = "How about adding your email address!?")]
    [EmailAddress(ErrorMessage = "How about adding your email address!?")]
    public string SenderAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "How about adding a subject!?")]
    [StringLength(ContactConsts.MaxSubjectCharacters, ErrorMessage = "That appears to be a tad long...")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "How about adding a few words!?")]
    [StringLength(ContactConsts.MaxMessageCharacters, ErrorMessage = "That appears to be a tad long...")]
    public string Message { get; set; } = string.Empty;

    [Required(ErrorMessage = "How about adding the verification code!?")]
    [Compare(nameof(VerificationCode), ErrorMessage = "That verification code doesn't look quite right...")]
    public string VerificationCodeEntered { get; set; } = string.Empty;

    public string VerificationCode { get; set; } = string.Empty;
}
