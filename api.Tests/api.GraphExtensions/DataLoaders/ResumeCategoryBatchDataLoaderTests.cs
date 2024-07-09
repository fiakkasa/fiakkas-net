using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.GraphExtensions.DataLoaders.Tests;

public class ResumeCategoryBatchDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.Resume,
                Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
            }
        ]);

        var sut = new ResumeCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d")], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().NotBeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Null_Item_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>();

        var sut = new ResumeCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().BeNull();
        result.MatchSnapshot();
    }
}
