using api.TextItems.Interfaces;

namespace api.TextItems.Models;

[ExcludeFromCodeCoverage]
public record TextItemEntity : BaseData, ITextItem
{
    public string Key { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}
