using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Mappers;

public class PortfolioItemMappersTests
{
    public record TestPortfolioItem : IPortfolioItem
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public long Year { get; init; }
        public Guid CategoryId { get; init; }
        public long Ordinal { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
        public Guid[] TechnologyIds { get; init; } = [];
        public Guid CustomerId { get; init; }
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new TestPortfolioItem
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = Guid.Empty,
            Ordinal = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [Guid.Empty],
            CustomerId = Guid.Empty
        };

        var result = item.Map();

        result.Should().BeOfType<PortfolioItem>();
        result.Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
