using api.EducationItems.Interfaces;
using api.EducationItems.Models;

namespace api.GraphExtensions.DataLoaders.Tests;

public class EducationItemByResumeCategoryIdGroupDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IEducationItem<EducationTimePeriod>>(
        [
            new EducationItem
            {
                Id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                CategoryId = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                TimePeriod = new()
                {
                    Start = new DateOnly(2024, 1, 1),
                    End = null
                },
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                Location = "Location",
                Description = "Description",
                Subjects = ["Subject"]
            }
        ]);
        var sut = new EducationItemByResumeCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d")], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadGroupedBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<IEducationItem<EducationTimePeriod>>([]);

        var sut = new EducationItemByResumeCategoryIdGroupDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
