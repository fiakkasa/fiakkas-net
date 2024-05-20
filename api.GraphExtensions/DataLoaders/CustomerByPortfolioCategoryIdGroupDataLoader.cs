namespace api.GraphExtensions.DataLoaders;

public class CustomerByPortfolioCategoryIdGroupDataLoader(
    IDataRepository<ICustomer> customerDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, Customer>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, Customer>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(() =>
            portfolioDataRepository.Get()
                .Where(x => keys.Contains(x.CategoryId))
                .Select(x => new { x.CustomerId, x.CategoryId })
                .Distinct()
                .Join(
                    customerDataRepository.Get(),
                    x => x.CustomerId,
                    item => item.Id,
                    (x, item) => new { x.CategoryId, item }
                )
                .ToLookup(x => x.CategoryId, x => x.item.Map()),
            cancellationToken
        );
}
