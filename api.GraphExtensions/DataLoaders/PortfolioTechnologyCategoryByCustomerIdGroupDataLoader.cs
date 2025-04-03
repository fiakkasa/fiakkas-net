namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioTechnologyCategoryByCustomerIdGroupDataLoader(
    IDataRepository<ICategory> categoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : GroupedDataLoader<Guid, IPolymorphicTechnologyCategory>(batchScheduler, options)
{
    private static readonly Expression<Func<ICategory, bool>> _where = x =>
        CategoryEntityUtils.IsTechnologyCategory(x);

    protected override async Task<ILookup<Guid, IPolymorphicTechnologyCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) => await Task.Run(() =>
        {
            var collection =
                portfolioDataRepository
                    .Get()
                    .Where(x => keys.Contains(x.CustomerId))
                    .SelectMany(item => item.TechnologyIds.Select(techId => new
                    {
                        techId,
                        item.CustomerId
                    }))
                    .ToHashSet();
            var items =
                categoryDataRepository
                    .Get()
                    .Where(_where)
                    .ToHashSet();

            return collection
                .Join(
                    items,
                    x => x.techId,
                    item => item.Id,
                    (x, item) => new
                    {
                        x.CustomerId,
                        item
                    }
                )
                .ToLookup(x => x.CustomerId, x => x.item.MapPolymorphicTechnologyCategory());
        },
        cancellationToken
    );
}
