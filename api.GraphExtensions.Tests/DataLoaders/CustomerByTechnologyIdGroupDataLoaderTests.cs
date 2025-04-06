using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.Tests.DataLoaders;

public class CustomerByTechnologyIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var customerDataRepository = new MockDataRepository<ICustomer>(
        [
            new Customer
            {
                Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        ]);
        var portfolioItemDataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = new("28e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Year = 2024,
                CategoryId = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                Title = "Title",
                Href = new("/test", UriKind.Relative),
                TechnologyIds = [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                CustomerId = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]);
        var sut = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioItemDataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync([new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")], CancellationToken.None);

        Assert.Single(result);
        Assert.Single(result[0]!);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found()
    {
        var customerDataRepository = new MockDataRepository<ICustomer>();
        var portfolioItemDataRepository = new MockDataRepository<IPortfolioItem>();

        var sut = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioItemDataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(result[0]!);
        result.MatchSnapshot();
    }
}
