namespace api.GraphExtensions.DataLoaders;

public sealed class TechnologyCategoryGroupDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, IPolymorphicTechnologyCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, IPolymorphicTechnologyCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            x =>
                CategoryEntityUtils.IsTechnologyCategory(x)
                && keys.Contains(x.Id),
            x => x.Id,
            CategoryMappers.MapPolymorphicTechnologyCategory,
            cancellationToken
        );
}
