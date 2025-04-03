using api.Achievements.Interfaces;
using api.Achievements.Mappers;
using api.Achievements.Models;

namespace api.Achievements.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class AchievementBatchDataLoader(
    IDataRepository<IAchievement> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : AbstractGenericBatchDataLoaderById<IAchievement, Achievement>(
    dataRepository,
    AchievementMappers.Map,
    batchScheduler,
    options
);
