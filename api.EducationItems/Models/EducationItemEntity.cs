using api.EducationItems.Interfaces;

namespace api.EducationItems.Models;

[ExcludeFromCodeCoverage]
public record EducationItemEntity : BaseData, IEducationItem
{
    public Guid CategoryId { get; init; }
    public TimePeriod TimePeriod { get; init; } = new();
    public string Title { get; init; } = string.Empty;
    public Uri? Href { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string[] Subjects { get; init; } = [];
}
