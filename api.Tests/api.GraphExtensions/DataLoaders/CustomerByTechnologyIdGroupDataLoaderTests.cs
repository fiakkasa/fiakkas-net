using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.TestingShared;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.DataLoaders;

public class CustomerByTechnologyIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var customerDataRepository = new MockDataRepository<ICustomer>(
        [
            new Customer
            {
                Id = Guid.Empty,
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var portfolioItemDataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        ]);

        var sut = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioItemDataRepository,
            AutoBatchScheduler.Default
        );

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadGroupedBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var customerDataRepository = new MockDataRepository<ICustomer>();
        var portfolioItemDataRepository = new MockDataRepository<IPortfolioItem>();

        var sut = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioItemDataRepository,
            AutoBatchScheduler.Default
        );

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
