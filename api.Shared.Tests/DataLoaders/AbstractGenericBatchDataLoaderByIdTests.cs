using api.Shared.DataLoaders;
using api.Shared.Types.Interfaces;
using api.Testing.Shared.Services;
using GreenDonut;

namespace api.Shared.Tests.DataLoaders;

public class AbstractGenericBatchDataLoaderByIdTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109");
        var dataRepository = new MockDataRepository<IMockItem>(
        [
            new MockItemEntity
            {
                Id = id,
                Text = "Text"
            }
        ]);
        var sut = new MockBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([id], CancellationToken.None);

        Assert.Single(result);
        Assert.NotNull(result[0]);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IMockItem>();

        var sut = new MockBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        Assert.Single(result);
        Assert.Null(result[0]);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found_With_Predicate()
    {
        var id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109");
        var dataRepository = new MockDataRepository<IMockItem>(
        [
            new MockItemEntity
            {
                Id = id,
                Text = "Hello"
            }
        ]);
        var sut = new MockWithPredicateBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([id], CancellationToken.None);

        Assert.Single(result);
        Assert.NotNull(result[0]);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found_With_Predicate()
    {
        var dataRepository = new MockDataRepository<IMockItem>();

        var sut = new MockWithPredicateBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        Assert.Single(result);
        Assert.Null(result[0]);
        result.MatchSnapshot();
    }

    public interface IMockItem : IBaseId
    {
        string Text { get; }
    }

    private record MockItemEntity : IMockItem
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
    }

    public record MockItem : IMockItem
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
    }

    public sealed class MockBatchDataLoader(
        IDataRepository<IMockItem> dataRepository,
        IBatchScheduler batchScheduler
    ) : AbstractGenericBatchDataLoaderById<IMockItem, MockItem>(
        dataRepository,
        x => new()
        {
            Id = x.Id,
            Text = x.Text
        },
        batchScheduler,
        new()
    );

    public sealed class MockWithPredicateBatchDataLoader(
        IDataRepository<IMockItem> dataRepository,
        IBatchScheduler batchScheduler
    ) : AbstractGenericBatchDataLoaderById<IMockItem, MockItem>(
        dataRepository,
        x => new()
        {
            Id = x.Id,
            Text = x.Text
        },
        batchScheduler,
        new(),
        (x, keys) => keys.Contains(x.Id) && x.Text == "Hello"
    );
}
