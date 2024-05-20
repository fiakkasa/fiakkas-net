namespace api.Technologies.Models;

[ExcludeFromCodeCoverage]
public record TechnologyDataConfig
{
    public TechnologyEntity[] Technologies { get; init; } = [];
}
