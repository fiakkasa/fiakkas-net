using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.Tests.DataLoaders;

public class TechnologyCategoryGroupDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new("ca732bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
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
                Kind = CategoryType.InformationTechnology,
                Id = new("cb832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        ]);
        var sut = new TechnologyCategoryGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result =
            await sut.LoadAsync(
                [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"), new("cb832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                CancellationToken.None);

        result.Should().HaveCount(2);
        result.All(x => x is { Length: 1 }).Should().BeTrue();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICategory>();

        var sut = new TechnologyCategoryGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Should().BeEmpty();
        result.MatchSnapshot();
    }
}
