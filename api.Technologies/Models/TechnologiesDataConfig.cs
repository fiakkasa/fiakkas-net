namespace api.Technologies.Models;

[ExcludeFromCodeCoverage]
public record TechnologiesDataConfig
{
    public TechnologyEntity[] Technologies { get; init; } = [];
}
