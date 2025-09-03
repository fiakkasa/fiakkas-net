namespace api.Shared.Types.Interfaces;

public interface ITimePeriod
{
    DateOnly Start { get; init; }
    DateOnly? End { get; init; }
}
