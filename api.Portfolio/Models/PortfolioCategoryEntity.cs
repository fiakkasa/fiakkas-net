using api.Portfolio.Interfaces;

namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioCategoryEntity : BaseData, IPortfolioCategory
{
    public required string Title { get; init; }
    public Uri? Href { get; init; }
}
