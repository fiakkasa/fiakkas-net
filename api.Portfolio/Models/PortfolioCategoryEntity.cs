using api.Portfolio.Interfaces;

namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioCategoryEntity : BaseData, IPortfolioCategory
{
    public string Title { get; init; } = string.Empty;
}
