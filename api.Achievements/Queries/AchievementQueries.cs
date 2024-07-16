using api.Achievements.Interfaces;
using api.Achievements.Mappers;
using api.Achievements.Models;

namespace api.Achievements.Queries;

[QueryType]
public static class AchievementQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Achievement> GetAchievements([Service] IDataRepository<IAchievement> repository) =>
        repository.Get(AchievementMappers.Map);
}
