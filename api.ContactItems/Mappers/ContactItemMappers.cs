using api.ContactItems.Interfaces;
using api.ContactItems.Models;

namespace api.ContactItems.Mappers;

public static class ContactItemMappers
{
    public static ContactItem Map(this IContactItem x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Key = x.Key,
            Icon = x.Icon,
            Title = x.Title,
            Description = x.Description,
            Href = x.Href
        };
}
