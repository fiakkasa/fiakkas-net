namespace api.GraphExtensions.DataLoaders;

public sealed class TechnologyBatchDataLoader(
    IDataRepository<ITechnology> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, Technology>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Technology>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(keys, TechnologyMappers.Map, cancellationToken);
}
