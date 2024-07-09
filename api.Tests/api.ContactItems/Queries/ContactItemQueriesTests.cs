using api.ContactItems.Interfaces;
using api.ContactItems.Models;

namespace api.ContactItems.Queries.Tests;

public class ContactItemQueriesTests
{
    [Fact]
    public void GetContactItems_Should_Return_Data()
    {
        var item = new ContactItem
        {
            Id = new Guid("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Icon = "Icon",
            Title = "Title",
            Description = "Content",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<IContactItem>([item]);
        var sut = new ContactItemQueries();

        var result = sut.GetContactItems(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<ContactItem>>();
        result.MatchSnapshot();
    }
}
