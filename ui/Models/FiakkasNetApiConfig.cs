using Polly;

namespace ui.Models;

public record FiakkasNetApiConfig : IValidatableObject
{
    [Required]
    public Uri BaseUrl { get; init; } = default!;

    [EnumDataType(typeof(DelayBackoffType))]
    public DelayBackoffType DelayBackoffType { get; init; } = DelayBackoffType.Exponential;

    public bool UseJitter { get; init; } = true;

    [Range(1, 10)]
    public int MaxRetryAttempts { get; init; } = 3;

    public TimeSpan Delay { get; init; } = TimeSpan.FromMilliseconds(200);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Delay is not { TotalMilliseconds: >= 100 and <= 2_000 })
        {
            yield return new ValidationResult(
                "Delay must be between 100 milliseconds and 2 seconds inclusive.",
                [nameof(Delay)]
            );
        }
    }
}
