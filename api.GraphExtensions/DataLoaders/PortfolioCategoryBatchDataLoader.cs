namespace api.GraphExtensions.DataLoaders;

public sealed class PortfolioCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, PortfolioCategory>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, PortfolioCategory>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(
            x =>
                CategoryEntityUtils.IsPortfolioCategory(x)
                && keys.Contains(x.Id),
            CategoryMappers.MapGenericCategory<PortfolioCategory>,
            cancellationToken
        );
}
