using api.Shared.Types.Interfaces;
using api.Testing.Shared.Services;
using GreenDonut;

namespace api.Shared.DataLoaders.Tests;

public class GenericBatchDataLoaderByIdTests
{
    public interface IMockItem : IBaseId
    {
        string Text { get; }
    }

    public record MockItemEntity : IMockItem
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
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null
    ) : GenericBatchDataLoaderById<IMockItem, MockItem>(
        dataRepository,
        x => new MockItem { Id = x.Id, Text = x.Text },
        batchScheduler,
        options
    )
    { }

    public sealed class MockWithPredicateBatchDataLoader(
        IDataRepository<IMockItem> dataRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null
    ) : GenericBatchDataLoaderById<IMockItem, MockItem>(
        dataRepository,
        x => new MockItem { Id = x.Id, Text = x.Text },
        batchScheduler,
        options,
        (x, keys) => keys.Contains(x.Id) && x.Text == "Hello"
    )
    { }

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

        result.Should().ContainSingle();
        result[0].Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IMockItem>();

        var sut = new MockBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().BeNull();
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

        result.Should().ContainSingle();
        result[0].Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found_With_Predicate()
    {
        var dataRepository = new MockDataRepository<IMockItem>();

        var sut = new MockWithPredicateBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().BeNull();
        result.MatchSnapshot();
    }
}
