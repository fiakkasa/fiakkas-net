using api.Achievements.Models;

namespace api.Achievements.TypeExtensions;

[ExtendObjectType<Achievement>]
public sealed class AchievementTypeExtension
{
    public string GetYearsSummary([Parent] Achievement parent) => string.Join(", ", parent.Years);
}
