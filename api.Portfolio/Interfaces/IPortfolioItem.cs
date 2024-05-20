namespace api.Portfolio.Interfaces;

public interface IPortfolioItem : IBaseData
{
    long Year { get; set; }
    Guid CategoryId { get; set; }
    long Ordinal { get; set; }
    string Title { get; set; }
    Uri? Href { get; set; }
    Guid[] TechnologyIds { get; set; }
    Guid CustomerId { get; set; }
}
