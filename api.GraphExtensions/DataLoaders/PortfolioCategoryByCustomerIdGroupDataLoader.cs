namespace api.GraphExtensions.DataLoaders;

public class PortfolioCategoryByCustomerIdGroupDataLoader(
    IDataRepository<IPortfolioCategory> portfolioCategoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(() =>
            portfolioDataRepository.Get()
                .Where(x => keys.Contains(x.CustomerId))
                .Select(x => new { x.CategoryId, x.CustomerId })
                .Distinct()
                .Join(
                    portfolioCategoryDataRepository.Get(),
                    x => x.CategoryId,
                    item => item.Id,
                    (x, item) => new { x.CustomerId, item }
                )
                .ToLookup(x => x.CustomerId, x => x.item.Map()),
            cancellationToken
        );
}
