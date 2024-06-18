using api.ContactItems.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.ContactItems.Services.Tests;

public class ContactItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new ContactItemEntity
        {
            Id = new Guid("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Icon = "Icon",
            Title = "Title",
            Content = "Content",
            Href = new Uri("/test", UriKind.Relative)
        };
        var configData = new ContactItemsDataConfig
        {
            ContactItems = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<ContactItemsDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new ContactItemDataRepository(Substitute.For<ILogger<ContactItemDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
