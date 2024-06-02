using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Mappers;

public class PortfolioCategoryMappersTests
{
    public record PortfolioCategoryMockEntity : IPortfolioCategory
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new PortfolioCategoryMockEntity
        {
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.Map();

        result.Should().BeOfType<PortfolioCategory>();
        result.Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
