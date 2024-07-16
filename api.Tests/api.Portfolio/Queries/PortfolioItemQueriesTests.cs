using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Queries.Tests;

public class PortfolioItemQueriesTests
{
    [Fact]
    public void GetPortfolioItems_Should_Return_Data()
    {
        var item = new PortfolioItem
        {
            Id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
            CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };
        var service = new MockDataRepository<IPortfolioItem>([item]);

        var result = PortfolioItemQueries.GetPortfolioItems(service);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<PortfolioItem>>();
        result.MatchSnapshot();
    }
}
