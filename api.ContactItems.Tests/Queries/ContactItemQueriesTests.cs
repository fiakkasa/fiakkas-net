using api.ContactItems.DataLoaders;
using api.ContactItems.Interfaces;
using api.ContactItems.Models;
using api.ContactItems.Queries;
using GreenDonut;

namespace api.ContactItems.Tests.Queries;

public class ContactItemQueriesTests
{
    [Fact]
    public void GetContactItems_Should_Return_Data()
    {
        var item = new ContactItem
        {
            Id = new("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Icon = "Icon",
            Title = "Title",
            Description = "Content",
            Href = new("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<IContactItem>([item]);

        var result = ContactItemQueries.GetContactItems(dataRepository);

        Assert.Single(result);
        Assert.IsAssignableFrom<IQueryable<ContactItem>>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetContactItemById_Should_Return_Data_When_Found()
    {
        var id = new Guid("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05");
        var item = new ContactItem
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Icon = "Icon",
            Title = "Title",
            Description = "Content",
            Href = new("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<IContactItem>([item]);
        var dataLoader = new ContactItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await ContactItemQueries.GetContactItemById(
            id,
            dataLoader,
            default
        );

        Assert.NotNull(result);
        Assert.IsAssignableFrom<ContactItem>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetContactItemById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05");
        var dataRepository = new MockDataRepository<IContactItem>([]);
        var dataLoader = new ContactItemBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await ContactItemQueries.GetContactItemById(
            id,
            dataLoader,
            default
        );

        Assert.Null(result);
    }
}
