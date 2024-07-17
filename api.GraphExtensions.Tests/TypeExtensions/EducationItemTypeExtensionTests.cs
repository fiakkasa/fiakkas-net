using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.EducationItems.Models;
using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions.Tests;

public class EducationItemTypeExtensionTests
{
    [Fact]
    public async Task GetCategory_Should_Return_Data()
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
        var dataLoader = new ResumeCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new EducationItemTypeExtension();

        var result = await sut.GetCategory(
            new EducationItem { CategoryId = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }
}
