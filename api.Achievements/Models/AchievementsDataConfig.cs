namespace api.Achievements.Models;

[ExcludeFromCodeCoverage]
public record AchievementsDataConfig
{
    public AchievementEntity[] Achievements { get; init; } = [];
}
