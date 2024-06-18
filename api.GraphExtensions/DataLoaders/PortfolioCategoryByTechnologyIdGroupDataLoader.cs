namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioCategoryByTechnologyIdGroupDataLoader(
    IDataRepository<ICategoryEntity> categoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository
                        .Get()
                        .Where(x => x.TechnologyIds.Any(techId => keys.Contains(techId)))
                        .SelectMany(item => item.TechnologyIds.Select(techId => new { item.CategoryId, techId }))
                        .ToHashSet();
                var items =
                    categoryDataRepository
                        .Get()
                        .Where(CategoryEntityUtils.IsPortfolioCategory)
                        .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.CategoryId,
                        item => item.Id,
                        (x, item) => new { x.techId, item }
                    )
                    .ToLookup(x => x.techId, x => x.item.MapGenericCategory<PortfolioCategory>());
            },
            cancellationToken
        );
}
