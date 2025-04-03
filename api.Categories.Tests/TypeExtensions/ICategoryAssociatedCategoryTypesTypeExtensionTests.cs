using api.Categories.DataLoaders;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.Categories.TypeExtensions;

namespace api.Categories.Tests.TypeExtensions;

public class ICategoryAssociatedCategoryTypesTypeExtensionTests
{
    [Fact]
    public async Task GetAssociatedCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
        [
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
                Kind = CategoryType.Other,
                Id = new("cb832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);
        var dataLoader = new AssociatedCategoryGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );
        var sut = new ICategoryAssociatedCategoryTypesTypeExtension();

        var result = await sut.GetAssociatedCategories(
            new ResumeCategory
            {
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment, CategoryType.Other]
            },
            dataLoader,
            CancellationToken.None
        );

        result.Should().HaveCount(2);
        result.MatchSnapshot();
    }
}
