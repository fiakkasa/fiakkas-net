using api.Technologies.Interfaces;

namespace api.Technologies.Models;

[ExcludeFromCodeCoverage]
public record TechnologyEntity : BaseData, ITechnology
{
    public string Title { get; init; } = string.Empty;
    public Uri? Href { get; init; }
}
