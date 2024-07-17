using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.TypeExtensions.Tests;

public class ITechnologyCategoryTypeExtensionTests
{
    [Fact]
    public async Task GetPortfolioCategories_Should_Return_Data()
    {
        var categoryDataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.Portfolio,
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
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]);
        var dataLoader = new PortfolioCategoryByTechnologyIdGroupDataLoader(
            categoryDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new ITechnologyCategoryTypeExtension();

        var result = await sut.GetPortfolioCategories(
            new SoftwareDevelopmentCategory { Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().ContainSingle();
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
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]);
        var dataLoader = new CustomerByTechnologyIdGroupDataLoader(
            customerDataRepository,
            portfolioDataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new ITechnologyCategoryTypeExtension();

        var result = await sut.GetPortfolioCustomers(
            new SoftwareDevelopmentCategory { Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().ContainSingle();
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
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]);
        var dataLoader = new PortfolioItemByTechnologyIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new ITechnologyCategoryTypeExtension();

        var result = await sut.GetPortfolioItems(
            new SoftwareDevelopmentCategory { Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }
}
