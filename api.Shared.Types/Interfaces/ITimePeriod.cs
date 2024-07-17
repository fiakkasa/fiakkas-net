namespace api.Shared.Types.Interfaces;

public interface ITimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
