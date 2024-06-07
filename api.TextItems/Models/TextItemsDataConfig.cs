namespace api.TextItems.Models;

[ExcludeFromCodeCoverage]
public record TextItemsDataConfig
{
    public TextItemEntity[] TextItems { get; init; } = [];
}
