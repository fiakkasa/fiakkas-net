namespace api.GraphExtensions.DataLoaders;

public sealed class TechnologyCategoryGroupDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, ITechnologyCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, ITechnologyCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            x =>
                CategoryEntityUtils.IsTechnologyCategory(x)
                && keys.Contains(x.Id),
            x => x.Id,
            CategoryMappers.MapTechnologyCategories,
            cancellationToken
        );
}
