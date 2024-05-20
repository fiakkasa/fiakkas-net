using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Mappers;

public static class TechnologyMappers
{
    public static Technology Map(this ITechnology x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            Href = x.Href
        };
}
