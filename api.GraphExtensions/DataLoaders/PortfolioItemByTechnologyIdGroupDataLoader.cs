namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioItemByTechnologyIdGroupDataLoader(
    IDataRepository<IPortfolioItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioItem>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioItem>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(() =>
            dataRepository
                .Get()
                .Where(x => x.TechnologyIds.Any(techId => keys.Contains(techId)))
                .SelectMany(item => item.TechnologyIds.Select(techId => new { item, techId }))
                .ToLookup(x => x.techId, x => x.item.Map()),
            cancellationToken
        );
}
