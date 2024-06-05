namespace api.GraphExtensions.DataLoaders;

public sealed class TechnologyByPortfolioCategoryIdGroupDataLoader(
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
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository.Get()
                        .Where(x => keys.Contains(x.CategoryId))
                        .SelectMany(item => item.TechnologyIds.Select(techId => new { techId, item.CategoryId }))
                        .ToHashSet();
                var ids = collection.Select(x => x.techId).ToHashSet();
                var items =
                    technologyDataRepository.Get()
                        .Where(x => ids.Contains(x.Id))
                        .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.techId,
                        item => item.Id,
                        (x, item) => new { x.CategoryId, item }
                    )
                    .ToLookup(x => x.CategoryId, x => x.item.Map());
            },
            cancellationToken
        );
}
