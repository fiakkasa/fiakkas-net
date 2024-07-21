using api.EducationItems.DataLoaders;
using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using GreenDonut;

namespace api.EducationItems.Queries.Tests;

public class EducationItemQueriesTests
{
    [Fact]
    public void GetEducationItems_Should_Return_Data()
    {
        var item = new EducationItem
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
        };
        var dataRepository = new MockDataRepository<IEducationItem>([item]);

        var result = EducationItemQueries.GetEducationItems(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<EducationItem>>();
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
        };
        var dataRepository = new MockDataRepository<IEducationItem>([item]);
        var dataLoader = new EducationItemBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await EducationItemQueries.GetEducationItemById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeOfType<EducationItem>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetEducationItemById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224");
        var dataRepository = new MockDataRepository<IEducationItem>([]);
        var dataLoader = new EducationItemBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await EducationItemQueries.GetEducationItemById(id, dataLoader, default);

        result.Should().BeNull();
    }
}
