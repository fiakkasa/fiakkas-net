using api.Achievements.DataLoaders;
using api.Achievements.Interfaces;
using api.Achievements.Mappers;
using api.Achievements.Models;

namespace api.Achievements.Queries;

[QueryType]
public static class AchievementQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Achievement> GetAchievements([Service] IDataRepository<IAchievement> repository) =>
        repository.Get(AchievementMappers.Map);

    [NodeResolver]
    public static async ValueTask<Achievement?> GetAchievementById(
        Guid id,
        AchievementBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
