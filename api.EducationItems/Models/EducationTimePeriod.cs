using api.EducationItems.Interfaces;

namespace api.EducationItems.Models;

public record EducationTimePeriod : IEducationTimePeriod
{
    public DateOnly Start { get; init; }
    public DateOnly? End { get; init; }
}
