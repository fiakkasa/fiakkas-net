namespace api.Portfolio.Interfaces;

public interface IPortfolioItem : IBaseData
{
    long Year { get; init; }
    Guid CategoryId { get; init; }
    long Ordinal { get; init; }
    string Title { get; init; }
    Uri? Href { get; init; }
    Guid[] TechnologyIds { get; init; }
    Guid CustomerId { get; init; }
}
