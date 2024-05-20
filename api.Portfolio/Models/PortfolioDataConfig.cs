namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioDataConfig
{
    public PortfolioCategoryEntity[] PortfolioCategories { get; init; } = [];
    public PortfolioItemEntity[] PortfolioItems { get; init; } = [];
}
