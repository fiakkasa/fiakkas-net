using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.Queries;

public class PortfolioCategoryQueriesTests
{
    [Fact]
    public void GetPortfolioItems_Should_Return_Result()
    {
        var item = new PortfolioCategory
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var service = Substitute.For<IDataRepository<IPortfolioCategory>>();
        service
            .Get(Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>())
            .Returns(new[] { item }.AsQueryable());
        var qut = new PortfolioCategoryQueries();

        var result = qut.GetPortfolioCategories(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<PortfolioCategory>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
