namespace api.GraphExtensions.DataLoaders;

public class PortfolioCategoryByTechnologyIdGroupDataLoader(
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
                .Select(item => item.TechnologyIds.Select(techId => new { item.CategoryId, techId }))
                .SelectMany(x => x)
                .Distinct()
                .Where(x => keys.Contains(x.techId))
                .Join(
                    portfolioCategoryDataRepository.Get(),
                    x => x.CategoryId,
                    item => item.Id,
                    (x, item) => new { x.techId, item }
                )
                .ToLookup(x => x.techId, x => x.item.Map()),
            cancellationToken
        );
}
