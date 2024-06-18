namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioTechnologyCategoryByPortfolioCategoryIdGroupDataLoader(
    IDataRepository<ICategoryEntity> categoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, ITechnologyCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, ITechnologyCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await Task.Run(
            () =>
            {
                var collection =
                    portfolioDataRepository
                        .Get()
                        .Where(x => keys.Contains(x.CategoryId))
                        .SelectMany(item => item.TechnologyIds.Select(techId => new { techId, item.CategoryId }))
                        .ToHashSet();
                var items =
                    categoryDataRepository
                        .Get()
                        .Where(CategoryEntityUtils.IsTechnologyCategory)
                        .ToHashSet();

                return collection
                    .Join(
                        items,
                        x => x.techId,
                        item => item.Id,
                        (x, item) => new { x.CategoryId, item }
                    )
                    .ToLookup(x => x.CategoryId, x => x.item.MapTechnologyCategories());
            },
            cancellationToken
        );
}
