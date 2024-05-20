namespace api.GraphExtensions.DataLoaders;

public class CustomerBatchDataLoader(
    IDataRepository<ICustomer> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, Customer>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Customer>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(keys, CustomerMappers.Map, cancellationToken);
}
