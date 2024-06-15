namespace api.ContactItems.Models;

[ExcludeFromCodeCoverage]
public record ContactItemsDataConfig
{
    public ContactItemEntity[] ContactItems { get; init; } = [];
}
