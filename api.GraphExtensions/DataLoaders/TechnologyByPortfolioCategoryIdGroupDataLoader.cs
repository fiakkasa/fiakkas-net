namespace api.GraphExtensions.DataLoaders;

public class TechnologyByPortfolioCategoryIdGroupDataLoader(
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
                .Where(x => keys.Contains(x.CategoryId))
                .Select(item => item.TechnologyIds.Select(techId => new { techId, item.CategoryId }))
                .SelectMany(x => x)
                .Distinct()
                .Join(
                    technologyDataRepository.Get(),
                    x => x.techId,
                    item => item.Id,
                    (x, item) => new { x.CategoryId, item }
                )
                .ToLookup(x => x.CategoryId, x => x.item.Map()),
            cancellationToken
        );
}
