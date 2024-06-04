using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.Portfolio.Models;
using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.GraphExtensions.TypeExtensions;

public class PortfolioItemTypeExtensionTests
{
    [Fact]
    public async Task GetCategory_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
        var dataLoader = new CategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetCategory(
            new PortfolioItem { CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCustomer_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICustomer>(
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
        var dataLoader = new CustomerBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetCustomer(
            new PortfolioItem { CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109") },
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
                Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
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
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetTechnologies(
            new PortfolioItem { TechnologyIds = [new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109")] },
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
                Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
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
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetTechnologiesSummary(
            new PortfolioItem { TechnologyIds = [new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109")] },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
