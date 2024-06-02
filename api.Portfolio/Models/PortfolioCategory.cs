using api.Portfolio.Interfaces;

namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioCategory : BaseData, IPortfolioCategory
{
    public string Title { get; init; } = string.Empty;
}
