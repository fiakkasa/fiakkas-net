using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TestingShared;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Technologies.Models;

namespace api.GraphExtensions.TypeExtensions;

public class TechnologyTypeExtensionTests
{
    [Fact]
    public async Task GetPortfolioCategories_Should_Return_Data()
    {
        var portfolioCategoryDataRepository = new MockDataRepository<IPortfolioCategory>(
        [
            new PortfolioCategory
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var portfolioDataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        ]);
        var dataLoader = new PortfolioCategoryByTechnologyIdGroupDataLoader(
            portfolioCategoryDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new TechnologyTypeExtension();

        var result = await teut.GetPortfolioCategories(
            new Technology { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCustomers_Should_Return_Data()
    {
        var customerDataRepository = new MockDataRepository<ICustomer>(
        [
            new Customer
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var portfolioDataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        ]);
        var dataLoader = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new TechnologyTypeExtension();

        var result = await teut.GetCustomers(
            new Technology { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioItems_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        ]);
        var dataLoader = new PortfolioItemByTechnologyIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new TechnologyTypeExtension();

        var result = await teut.GetPortfolioItems(
            new Technology { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
