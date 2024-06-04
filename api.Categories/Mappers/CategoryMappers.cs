using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers;

public static class CategoryMappers
{
    public static Category Map(this ICategory x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title
        };
}
