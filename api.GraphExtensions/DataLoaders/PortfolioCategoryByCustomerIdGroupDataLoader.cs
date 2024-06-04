namespace api.GraphExtensions.DataLoaders;

public class PortfolioCategoryByCustomerIdGroupDataLoader(
    IDataRepository<ICategory> portfolioCategoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, Category>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, Category>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository.Get()
                        .Where(x => keys.Contains(x.CustomerId))
                        .Select(x => new { x.CategoryId, x.CustomerId })
                        .ToHashSet();
                var ids = collection.Select(x => x.CategoryId).ToHashSet();
                var items =
                    portfolioCategoryDataRepository.Get()
                        .Where(x => ids.Contains(x.Id))
                        .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.CategoryId,
                        item => item.Id,
                        (x, item) => new { x.CustomerId, item }
                    )
                    .ToLookup(x => x.CustomerId, x => x.item.Map());
            },
            cancellationToken
        );
}
