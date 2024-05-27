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
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository.Get()
                        .Where(x => x.TechnologyIds.Any(techId => keys.Contains(techId)))
                        .SelectMany(item => item.TechnologyIds.Select(techId => new { item.CustomerId, techId }))
                        .ToHashSet();
                var ids = collection.Select(x => x.CustomerId).ToHashSet();
                var items =
                    customerDataRepository.Get()
                        .Where(x => ids.Contains(x.Id))
                        .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.CustomerId,
                        item => item.Id,
                        (x, item) => new { x.techId, item }
                    )
                    .ToLookup(x => x.techId, x => x.item.Map());
            },
            cancellationToken
        );
}
