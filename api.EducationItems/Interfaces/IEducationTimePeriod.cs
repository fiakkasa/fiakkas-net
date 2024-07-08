namespace api.EducationItems.Interfaces;

public interface IEducationTimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
