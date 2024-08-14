using api.Achievements.Interfaces;
using api.Achievements.Models;

namespace api.Achievements.Services;

public sealed class AchievementDataRepository(
    ILogger<AchievementDataRepository> logger,
    IOptionsSnapshot<AchievementsDataConfig> dataSnapshot
)
    : AbstractInMemoryDataRepository<IAchievement, AchievementsDataConfig>(logger, dataSnapshot)
{
    protected override IAchievement[]? ResolveSet(AchievementsDataConfig data) => data.Achievements;
}
