namespace api.Portfolio.Interfaces;

public interface IPortfolioCategory : IBaseData
{
    string Title { get; init; }
    Uri? Href { get; init; }
}
