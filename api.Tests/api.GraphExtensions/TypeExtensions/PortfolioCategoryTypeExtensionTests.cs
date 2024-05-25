using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TestingShared;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.GraphExtensions.TypeExtensions;

public class PortfolioCategoryTypeExtensionTests
{
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
        var dataLoader = new CustomerByPortfolioCategoryIdGroupDataLoader(
            customerDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioCategoryTypeExtension();

        var result = await teut.GetCustomers(
            new PortfolioCategory { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologies_Should_Return_Data()
    {
        var technologyDataRepository = new MockDataRepository<ITechnology>(
        [
            new Technology
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
        var dataLoader = new TechnologyByPortfolioCategoryIdGroupDataLoader(
            technologyDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioCategoryTypeExtension();

        var result = await teut.GetTechnologies(
            new PortfolioCategory { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
