namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioDataConfig
{
    public PortfolioItemEntity[] PortfolioItems { get; init; } = [];
}
