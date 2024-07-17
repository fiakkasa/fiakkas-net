using api.Shared.Types.Interfaces;

namespace api.Shared.Types.Models;

[ExcludeFromCodeCoverage]
public record TimePeriod : ITimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
