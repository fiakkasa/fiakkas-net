using api.Shared.Interfaces;

namespace api.Shared.Models;

[ExcludeFromCodeCoverage]
public record TimePeriod : ITimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
