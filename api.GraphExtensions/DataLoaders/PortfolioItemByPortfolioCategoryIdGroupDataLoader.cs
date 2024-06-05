namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioItemByPortfolioCategoryIdGroupDataLoader(
    IDataRepository<IPortfolioItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioItem>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioItem>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            keys,
            x => x.CategoryId,
            PortfolioItemMappers.Map,
            cancellationToken
        );
}
