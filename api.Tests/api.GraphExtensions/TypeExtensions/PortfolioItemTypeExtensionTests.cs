using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TestingShared;
using api.Portfolio.Models;
using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.GraphExtensions.TypeExtensions;

public class PortfolioItemTypeExtensionTests
{
    [Fact]
    public async Task GetCustomer_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICustomer>(
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
        var dataLoader = new CustomerBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioItemTypeExtension();

        var result = await teut.GetCustomer(
            new PortfolioItem { CustomerId = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologies_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ITechnology>(
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
        var dataLoader = new TechnologyBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioItemTypeExtension();

        var result = await teut.GetTechnologies(
            new PortfolioItem { TechnologyIds = [Guid.Empty] },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologiesSummary_Should_Return_Content()
    {
        var dataRepository = new MockDataRepository<ITechnology>(
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
        var dataLoader = new TechnologyBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioItemTypeExtension();

        var result = await teut.GetTechnologiesSummary(
            new PortfolioItem { TechnologyIds = [Guid.Empty] },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
