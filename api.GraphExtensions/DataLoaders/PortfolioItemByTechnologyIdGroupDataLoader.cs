namespace api.GraphExtensions.DataLoaders;

public class PortfolioItemByTechnologyIdGroupDataLoader(
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
            dataRepository.Get()
                .Select(item => item.TechnologyIds.Select(techId => new { item, techId }))
                .SelectMany(x => x)
                .Where(x => keys.Contains(x.techId))
                .ToLookup(x => x.techId, x => x.item.Map()),
            cancellationToken
        );
}
