using api.Categories.DataLoaders;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.DataLoaders;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;

namespace api.GraphExtensions.Tests.TypeExtensions;

public class PortfolioItemTypeExtensionTests
{
    [Fact]
    public async Task GetCategory_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
        var dataLoader = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetCategory(
            new()
            {
                CategoryId = new("38e483e4-6961-4b25-88a9-d1d0a5161109")
            },
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
                Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        ]);
        var dataLoader = new CustomerBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetCustomer(
            new()
            {
                CustomerId = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
            },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologyCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
        var dataLoader = new TechnologyCategoryGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetTechnologyCategories(
            new()
            {
                TechnologyIds = [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")]
            },
            dataLoader,
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologiesSummary_Should_Return_Content()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
        var dataLoader = new TechnologyCategoryGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetTechnologiesSummary(
            new()
            {
                TechnologyIds = [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")]
            },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
