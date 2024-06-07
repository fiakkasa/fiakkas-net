using api.TextItems.Interfaces;
using api.TextItems.Models;

namespace api.TextItems.Mappers;

public static class TextItemMappers
{
    public static TextItem Map(this ITextItem x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Key = x.Key,
            Title = x.Title,
            Content = x.Content
        };
}
