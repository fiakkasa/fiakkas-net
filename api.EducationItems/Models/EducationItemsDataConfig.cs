namespace api.EducationItems.Models;

[ExcludeFromCodeCoverage]
public record EducationItemsDataConfig
{
    public EducationItemEntity[] EducationItems { get; init; } = [];
}
