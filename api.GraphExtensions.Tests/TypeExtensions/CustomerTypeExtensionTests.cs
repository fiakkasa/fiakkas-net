using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.Tests.TypeExtensions;

public class CustomerTypeExtensionTests
{
    [Fact]
    public async Task GetPortfolioCategories_Should_Return_Data()
    {
        var categoryDataRepository = new MockDataRepository<ICategory>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.Portfolio,
                Id = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
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
        var dataLoader = new PortfolioCategoryByCustomerIdGroupDataLoader(
            categoryDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new CustomerTypeExtension();

        var result = await sut.GetPortfolioCategories(
            new()
            {
                Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
            },
            dataLoader,
            CancellationToken.None
        );

        Assert.NotNull(result);
        Assert.Single(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioTechnologyCategories_Should_Return_Data()
    {
        var categoryDataRepository = new MockDataRepository<ICategory>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        ]);
        var portfolioDataRepository = new MockDataRepository<IPortfolioItem>(
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
        var dataLoader = new PortfolioTechnologyCategoryByCustomerIdGroupDataLoader(
            categoryDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new CustomerTypeExtension();

        var result = await sut.GetPortfolioTechnologyCategories(
            new()
            {
                Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
            },
            dataLoader,
            CancellationToken.None
        );

        Assert.NotNull(result);
        Assert.Single(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioItems_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>(
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
        var dataLoader = new PortfolioItemByCustomerIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new CustomerTypeExtension();

        var result = await sut.GetPortfolioItems(
            new()
            {
                Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
            },
            dataLoader,
            CancellationToken.None
        );

        Assert.NotNull(result);
        Assert.Single(result);
        result.MatchSnapshot();
    }
}
