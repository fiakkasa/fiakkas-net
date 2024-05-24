using api.Portfolio.Interfaces;

namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioItemEntity : BaseData, IPortfolioItem
{
    public long Year { get; init; }
    public Guid CategoryId { get; init; }
    public long Ordinal { get; init; }
    public required string Title { get; init; }
    public Uri? Href { get; init; }
    public Guid[] TechnologyIds { get; init; } = [];
    public Guid CustomerId { get; init; }
}
