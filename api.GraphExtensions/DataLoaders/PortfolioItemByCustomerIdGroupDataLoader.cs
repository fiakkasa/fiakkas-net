namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioItemByCustomerIdGroupDataLoader(
    IDataRepository<IPortfolioItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : GroupedDataLoader<Guid, PortfolioItem>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioItem>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            keys,
            x => x.CustomerId,
            PortfolioItemMappers.Map,
            cancellationToken
        );
}
