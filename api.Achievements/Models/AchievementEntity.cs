using api.Achievements.Interfaces;

namespace api.Achievements.Models;

[ExcludeFromCodeCoverage]
public record AchievementEntity : AbstractBaseData, IAchievement
{
    public string Content { get; init; } = string.Empty;
    public int[] Years { get; init; } = [];
}
