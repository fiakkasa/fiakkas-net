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
            Ordinal = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109")],
            CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };
        var service = new MockDataRepository<IPortfolioItem>([item]);
        var sut = new PortfolioItemQueries();

        var result = sut.GetPortfolioItems(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<PortfolioItem>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
