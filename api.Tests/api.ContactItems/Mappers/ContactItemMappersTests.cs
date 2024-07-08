using api.ContactItems.Interfaces;
using api.ContactItems.Models;

namespace api.ContactItems.Mappers.Tests;

public class ContactItemMappersTests
{
    public record ContactItemMockEntity : IContactItem
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Key { get; init; } = string.Empty;
        public string? Icon { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Uri? Href { get; init; }
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new ContactItemMockEntity
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

        var result = item.Map();

        result.Should().BeOfType<ContactItem>();
        result.MatchSnapshot();
    }
}
