using api.Achievements.Interfaces;
using api.Achievements.Models;

namespace api.Achievements.Mappers;

public static class AchievementMappers
{
    public static Achievement Map(this IAchievement x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Content = x.Content,
            Years = x.Years
        };
}
