using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.DataLoaders.Tests;

public class PortfolioItemByPortfolioCategoryIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
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
            },
            new PortfolioItem
            {
                Id = new Guid("29e483e4-6961-4b25-88a9-d1d0a5161109"),
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
        var sut = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109")], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().HaveCount(2);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>([]);

        var sut = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().BeEmpty();
        result.MatchSnapshot();
    }
}
