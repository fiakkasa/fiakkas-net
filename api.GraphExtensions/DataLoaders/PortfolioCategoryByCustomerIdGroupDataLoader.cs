namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioCategoryByCustomerIdGroupDataLoader(
    IDataRepository<ICategory> categoryDataRepository,
    IDataRepository<IPortfolioItem> portfolioDataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioCategory>(batchScheduler, options)
{
    private static readonly Expression<Func<ICategory, bool>> _where = x =>
        CategoryEntityUtils.IsPortfolioCategory(x);

    protected override async Task<ILookup<Guid, PortfolioCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) => await Task.Run(() =>
        {
            var collection =
                portfolioDataRepository
                    .Get()
                    .Where(x => keys.Contains(x.CustomerId))
                    .Select(x => new
                    {
                        x.CategoryId,
                        x.CustomerId
                    })
                    .ToHashSet();
            var items =
                categoryDataRepository
                    .Get()
                    .Where(_where)
                    .ToHashSet();

            return collection
                .Join(
                    items,
                    x => x.CategoryId,
                    item => item.Id,
                    (x, item) => new
                    {
                        x.CustomerId,
                        item
                    }
                )
                .ToLookup(x => x.CustomerId, x => x.item.MapPortfolioCategory());
        },
        cancellationToken
    );
}
