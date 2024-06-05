namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioCategoryByTechnologyIdGroupDataLoader(
    IDataRepository<ICategory> portfolioCategoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, Category>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, Category>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository.Get()
                        .Where(x => x.TechnologyIds.Any(techId => keys.Contains(techId)))
                        .SelectMany(item => item.TechnologyIds.Select(techId => new { item.CategoryId, techId }))
                        .ToHashSet();
                var ids = collection.Select(x => x.CategoryId).ToHashSet();
                var items =
                   portfolioCategoryDataRepository.Get()
                       .Where(x => ids.Contains(x.Id))
                       .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.CategoryId,
                        item => item.Id,
                        (x, item) => new { x.techId, item }
                    )
                    .ToLookup(x => x.techId, x => x.item.Map());
            },
            cancellationToken
        );
}
