using api.Achievements.Interfaces;
using api.Achievements.Models;

namespace api.Achievements.Services;

public sealed class AchievementDataRepository(
    ILogger<AchievementDataRepository> logger,
    IOptionsSnapshot<AchievementsDataConfig> dataSnapshot
) : AbstractReadOnlyInMemoryDataRepository<IAchievement, AchievementsDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<IAchievement>? ResolveSet(AchievementsDataConfig data) => data.Achievements;
}
