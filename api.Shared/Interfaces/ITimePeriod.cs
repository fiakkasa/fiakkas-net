namespace api.Shared.Interfaces;

public interface ITimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
