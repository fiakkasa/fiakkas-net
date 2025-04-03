using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Portfolio.Queries;
using GreenDonut;

namespace api.Portfolio.Tests.Queries;

public class PortfolioItemQueriesTests
{
    [Fact]
    public void GetPortfolioItems_Should_Return_Data()
    {
        var item = new PortfolioItem
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
        };
        var service = new MockDataRepository<IPortfolioItem>([item]);

        var result = PortfolioItemQueries.GetPortfolioItems(service);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<PortfolioItem>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioItemById_Should_Return_Data_When_Found()
    {
        var id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109");
        var item = new PortfolioItem
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            Title = "Title",
            Href = new("/test", UriKind.Relative),
            TechnologyIds = [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
            CustomerId = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };
        var dataRepository = new MockDataRepository<IPortfolioItem>([item]);
        var dataLoader = new PortfolioItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await PortfolioItemQueries.GetPortfolioItemById(
            id,
            dataLoader,
            default
        );

        result.Should().NotBeNull();
        result.Should().BeOfType<PortfolioItem>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioItemById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109");
        var dataRepository = new MockDataRepository<IPortfolioItem>([]);
        var dataLoader = new PortfolioItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await PortfolioItemQueries.GetPortfolioItemById(
            id,
            dataLoader,
            default
        );

        result.Should().BeNull();
    }
}
