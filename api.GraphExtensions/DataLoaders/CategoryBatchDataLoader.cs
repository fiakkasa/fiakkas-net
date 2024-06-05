namespace api.GraphExtensions.DataLoaders;

public sealed class CategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, Category>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Category>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(keys, CategoryMappers.Map, cancellationToken);
}
