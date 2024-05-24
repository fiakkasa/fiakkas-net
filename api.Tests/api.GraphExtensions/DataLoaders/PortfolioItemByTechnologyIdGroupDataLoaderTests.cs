using api.GraphExtensions.TestingShared;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.GraphExtensions.DataLoaders;

public class PortfolioItemByTechnologyIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>(
        [
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        ]);

        var sut = new PortfolioItemByTechnologyIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadGroupedBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IPortfolioItem>();

        var sut = new PortfolioItemByTechnologyIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
