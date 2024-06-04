using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.DataLoaders;

public class PortfolioItemByPortfolioCategoryIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
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
            ]
        );
        var sut = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109")], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadGroupedBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>([]);

        var sut = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
