using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Mappers.Tests;

public class PortfolioItemMappersTests
{
    public record PortfolioItemMockEntity : IPortfolioItem
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public long Year { get; init; }
        public Guid CategoryId { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
        public Guid[] TechnologyIds { get; init; } = [];
        public Guid CustomerId { get; init; }
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new PortfolioItemMockEntity
        {
            Id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
            CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };

        var result = item.Map();

        result.Should().BeOfType<PortfolioItem>();
        result.MatchSnapshot();
    }
}
