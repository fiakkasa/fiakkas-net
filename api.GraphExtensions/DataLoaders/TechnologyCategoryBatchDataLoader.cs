namespace api.GraphExtensions.DataLoaders;

public sealed class TechnologyCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, ITechnologyCategory>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, ITechnologyCategory>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(
            x =>
                CategoryEntityUtils.IsTechnologyCategory(x)
                && keys.Contains(x.Id),
            CategoryMappers.MapTechnologyCategories,
            cancellationToken
        );
}
