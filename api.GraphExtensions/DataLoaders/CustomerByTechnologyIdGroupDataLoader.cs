namespace api.GraphExtensions.DataLoaders;

public class CustomerByTechnologyIdGroupDataLoader(
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
                .Select(item => item.TechnologyIds.Select(techId => new { item.CustomerId, techId }))
                .SelectMany(x => x)
                .Distinct()
                .Where(x => keys.Contains(x.techId))
                .Join(
                    customerDataRepository.Get(),
                    x => x.CustomerId,
                    item => item.Id,
                    (x, item) => new { x.techId, item }
                )
                .ToLookup(x => x.techId, x => x.item.Map()),
            cancellationToken
        );
}
