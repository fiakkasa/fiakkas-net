namespace ui.Extensions;

public static class StringExtensions
{
    public static string ToPrettyErrorMessage(this Exception exception) => 
        exception.Message.ToPrettyErrorMessage();

    public static string ToPrettyErrorMessage(this ValidationResult validationResult) => 
        validationResult.ErrorMessage.ToPrettyErrorMessage();

    public static string ToPrettyErrorMessage(this string? message) => message switch
    {
        { Length: > 0 } => message.Transform(To.LowerCase, To.SentenceCase).Humanize(),
        _ => string.Empty
    };
}
