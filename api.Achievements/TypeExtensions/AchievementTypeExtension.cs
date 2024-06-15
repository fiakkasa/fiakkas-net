using api.Achievements.Interfaces;

namespace api.Achievements.TypeExtensions;

[ExtendObjectType<IAchievement>]
public sealed class AchievementTypeExtension
{
    public string GetYearsSummary([Parent] IAchievement parent) => string.Join(", ", parent.Years);
}
