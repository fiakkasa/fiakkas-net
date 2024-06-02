using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.DataLoaders;

public class PortfolioItemByPortfolioCategoryIdGroupDataLoaderTests
{
    public class MockPortfolioCategoryDataRepository(IPortfolioItem[] collection) : IDataRepository<IPortfolioItem>
    {
        public IQueryable<IPortfolioItem> Get() => throw new NotImplementedException();

        public IQueryable<TMapped> Get<TMapped>(Func<IPortfolioItem, TMapped> mapper) => throw new NotImplementedException();

        public ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
            IReadOnlyList<Guid> keys,
            Func<IPortfolioItem, TMapped> mapper,
            CancellationToken cancellationToken = default
        ) where TMapped : IBaseId
            => throw new NotImplementedException();

        public ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
            IReadOnlyList<Guid> keys,
            Func<IPortfolioItem, Guid> keySelector,
            Func<IPortfolioItem, TMapped> mapper,
            CancellationToken cancellationToken = default
        ) =>
            ValueTask.FromResult(
                collection.ToLookup(x => keySelector(x), x => mapper(x))
            );
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockPortfolioCategoryDataRepository(
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
        var dataRepository = new MockPortfolioCategoryDataRepository([]);

        var sut = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}