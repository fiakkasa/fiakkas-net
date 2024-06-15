using api.Achievements.Interfaces;
using api.Achievements.Models;

namespace api.Achievements.Mappers.Tests;

public class AchievementMappersTests
{
    public record AchievementMockEntity : IAchievement
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Content { get; init; } = string.Empty;
        public int[] Years { get; init; } = [];
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new AchievementMockEntity
        {
            Id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2024]
        };

        var result = item.Map();

        result.Should().BeOfType<Achievement>();
        result.Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
