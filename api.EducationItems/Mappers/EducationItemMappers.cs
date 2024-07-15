using api.EducationItems.Interfaces;
using api.EducationItems.Models;

namespace api.EducationItems.Mappers;

public static class EducationItemMappers
{
    public static EducationItem Map(this IEducationItem x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            CategoryId = x.CategoryId,
            TimePeriod = x.TimePeriod,
            Href = x.Href,
            Location = x.Location,
            Description = x.Description,
            Subjects = x.Subjects
        };
}
