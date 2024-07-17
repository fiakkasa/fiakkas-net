using api.TextItems.Interfaces;
using api.TextItems.Models;

namespace api.TextItems.Mappers.Tests;

public class TextItemMappersTests
{
    public record TextItemMockEntity : ITextItem
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Key { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new TextItemMockEntity
        {
            Id = new Guid("2f69e973-550b-4769-801a-e757807e6845"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Title = "Title",
            Content = "Content"
        };

        var result = item.Map();

        result.Should().BeOfType<TextItem>();
        result.MatchSnapshot();
    }
}
