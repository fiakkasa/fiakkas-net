using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.Queries;

public class PortfolioItemQueriesTests
{
    [Fact]
    public void GetPortfolioItems_Should_Return_Data()
    {
        var item = new PortfolioItem
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = Guid.Empty,
            Ordinal = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [Guid.Empty],
            CustomerId = Guid.Empty
        };
        var service = Substitute.For<IDataRepository<IPortfolioItem>>();
        service
            .Get(Arg.Any<Func<IPortfolioItem, PortfolioItem>>())
            .Returns(new[] { item }.AsQueryable());
        var qut = new PortfolioItemQueries();

        var result = qut.GetPortfolioItems(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<PortfolioItem>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
