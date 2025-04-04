using api.Categories.DataLoaders;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Tests.DataLoaders;

public class AssociatedCategoryGroupDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.Resume,
                Id = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
            },
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            },
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new("cb832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        ]);
        var sut = new AssociatedCategoryGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [CategoryType.SoftwareDevelopment],
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result[0].Should().HaveCount(2);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>();

        var sut = new AssociatedCategoryGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [CategoryType.SoftwareDevelopment],
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result[0].Should().BeEmpty();
        result.MatchSnapshot();
    }
}
