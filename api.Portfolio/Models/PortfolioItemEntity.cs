using api.Portfolio.Interfaces;

namespace api.Portfolio.Models;

[ExcludeFromCodeCoverage]
public record PortfolioItemEntity : BaseData, IPortfolioItem
{
    public long Year { get; set; }
    public Guid CategoryId { get; set; }
    public long Ordinal { get; set; }
    public required string Title { get; set; }
    public Uri? Href { get; set; }
    public Guid[] TechnologyIds { get; set; } = [];
    public Guid CustomerId { get; set; }
}
