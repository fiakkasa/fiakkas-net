using api.Technologies.Interfaces;

namespace api.Technologies.Models;

[ExcludeFromCodeCoverage]
public record Technology : BaseData, ITechnology
{
    public required string Title { get; init; }
    public Uri? Href { get; init; }
}
