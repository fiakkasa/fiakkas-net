using api.Categories.DataLoaders;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Tests.DataLoaders;

public class PortfolioCategoryBatchDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
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

        var sut = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [new("38e483e4-6961-4b25-88a9-d1d0a5161109")],
            CancellationToken.None
        );

        Assert.Single(result);
        Assert.NotNull(result[0]);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);

        var sut = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [Guid.NewGuid()],
            CancellationToken.None
        );

        Assert.Single(result);
        Assert.Null(result[0]);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Items_Present()
    {
        var dataRepository = new MockDataRepository<ICategory>();

        var sut = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [Guid.NewGuid()],
            CancellationToken.None
        );

        Assert.Single(result);
        Assert.Null(result[0]);
        result.MatchSnapshot();
    }
}
