using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Queries;

public class PortfolioCategoryQueriesTests
{
    [Fact]
    public void GetPortfolioItems_Should_Return_Data()
    {
        var item = new PortfolioCategory
        {
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var service = new MockDataRepository<IPortfolioCategory>([item]);
        var sut = new PortfolioCategoryQueries();

        var result = sut.GetPortfolioCategories(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<PortfolioCategory>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
