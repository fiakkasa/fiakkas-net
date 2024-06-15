using api.Achievements.Interfaces;
using api.Achievements.Mappers;
using api.Achievements.Models;

namespace api.Achievements.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class AchievementQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Achievement> GetAchievements([Service] IDataRepository<IAchievement> repository) =>
        repository.Get(AchievementMappers.Map);
}
