using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.Tests.DataLoaders;

public class EducationItemByResumeCategoryIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IEducationItem>(
        [
            new EducationItem
            {
                Id = new("38898c62-161e-40f2-8a9f-39bf1ff46224"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                CategoryId = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                TimePeriod = new()
                {
                    Start = new(2024, 1, 1),
                    End = null
                },
                Title = "Title",
                Href = new("/test", UriKind.Relative),
                Location = "Location",
                Description = "Description",
                Subjects = ["Subject"]
            },
            new EducationItem
            {
                Id = new("39898c62-161e-40f2-8a9f-39bf1ff46224"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                CategoryId = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                TimePeriod = new()
                {
                    Start = new(2024, 1, 1),
                    End = null
                },
                Title = "Title",
                Href = new("/test", UriKind.Relative),
                Location = "Location",
                Description = "Description",
                Subjects = ["Subject"]
            }
        ]);
        var sut = new EducationItemByResumeCategoryIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [new("eb9d6258-99c4-46bd-bd44-23d35b19965d")],
            CancellationToken.None
        );

        Assert.Single(result);
        Assert.Equal(2, result[0]?.Length);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadAsync_Should_Return_Collection_With_Single_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IEducationItem>([]);

        var sut = new EducationItemByResumeCategoryIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await sut.LoadAsync(
            [Guid.NewGuid()],
            CancellationToken.None
        );

        Assert.Single(result);
        Assert.Empty(result[0]!);
        result.MatchSnapshot();
    }
}
