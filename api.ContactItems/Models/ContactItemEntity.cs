using api.ContactItems.Interfaces;

namespace api.ContactItems.Models;

[ExcludeFromCodeCoverage]
public record ContactItemEntity : AbstractBaseData, IContactItem
{
    public string Key { get; init; } = string.Empty;
    public string? Icon { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Uri? Href { get; init; }
}
