using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Technologies.Models;

namespace api.GraphExtensions.TypeExtensions;

public class TechnologyTypeExtensionTests
{
    [Fact]
    public async Task GetPortfolioCategories_Should_Return_Data()
    {
        var categoryDataRepository = new MockDataRepository<ICategory>(
        [
            new Category
            {
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);
        var portfolioDataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
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
            }
        ]);
        var dataLoader = new PortfolioCategoryByTechnologyIdGroupDataLoader(
            categoryDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new TechnologyTypeExtension();

        var result = await sut.GetPortfolioCategories(
            new Technology { Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109") },
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
                Id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
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
            }
        ]);
        var dataLoader = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new TechnologyTypeExtension();

        var result = await sut.GetPortfolioCustomers(
            new Technology { Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109") },
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
            }
        ]);
        var dataLoader = new PortfolioItemByTechnologyIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new TechnologyTypeExtension();

        var result = await sut.GetPortfolioItems(
            new Technology { Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
