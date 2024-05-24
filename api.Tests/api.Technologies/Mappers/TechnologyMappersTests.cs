using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Mappers;

public class TechnologyMappersTests
{
    public record TestTechnology : ITechnology
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
    }

    [Fact]
    public void Map_Should_Return_Technology()
    {
        var item = new TestTechnology
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.Map();

        result.Should().BeOfType<Technology>();
        result.Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
