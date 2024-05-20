namespace api.GraphExtensions.DataLoaders;

public class TechnologyByCustomerIdGroupDataLoader(
    IDataRepository<ITechnology> technologyDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, Technology>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, Technology>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(() =>
            portfolioDataRepository.Get()
                .Where(x => keys.Contains(x.CustomerId))
                .Select(item => item.TechnologyIds.Select(techId => new { techId, item.CustomerId }))
                .SelectMany(x => x)
                .Distinct()
                .Join(
                    technologyDataRepository.Get(),
                    x => x.techId,
                    item => item.Id,
                    (x, item) => new { x.CustomerId, item }
                )
                .ToLookup(x => x.CustomerId, x => x.item.Map()),
            cancellationToken
        );
}
