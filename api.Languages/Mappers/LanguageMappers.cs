using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Mappers;

public static class LanguageMappers
{
    public static Language Map(this ILanguage x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Proficiency = x.Proficiency,
            Title = x.Title
        };
}
