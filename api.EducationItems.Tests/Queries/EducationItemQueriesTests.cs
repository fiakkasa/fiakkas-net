using api.EducationItems.DataLoaders;
using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.EducationItems.Queries;
using GreenDonut;

namespace api.EducationItems.Tests.Queries;

public class EducationItemQueriesTests
{
    [Fact]
    public void GetEducationItems_Should_Return_Data()
    {
        var item = new EducationItem
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
        };
        var dataRepository = new MockDataRepository<IEducationItem>([item]);

        var result = EducationItemQueries.GetEducationItems(dataRepository);

        Assert.Single(result);
        Assert.IsAssignableFrom<IQueryable<EducationItem>>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetEducationItemById_Should_Return_Data_When_Found()
    {
        var id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224");
        var item = new EducationItem
        {
            Id = id,
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
        };
        var dataRepository = new MockDataRepository<IEducationItem>([item]);
        var dataLoader = new EducationItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await EducationItemQueries.GetEducationItemById(
            id,
            dataLoader,
            default
        );

        Assert.NotNull(result);
        Assert.IsType<EducationItem>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetEducationItemById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224");
        var dataRepository = new MockDataRepository<IEducationItem>([]);
        var dataLoader = new EducationItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await EducationItemQueries.GetEducationItemById(
            id,
            dataLoader,
            default
        );

        Assert.Null(result);
    }
}
