using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.GraphExtensions.DataLoaders;

public class CategoryBatchDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>(
        [
            new Category
            {
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);

        var sut = new CategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109")], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>();

        var sut = new CategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
