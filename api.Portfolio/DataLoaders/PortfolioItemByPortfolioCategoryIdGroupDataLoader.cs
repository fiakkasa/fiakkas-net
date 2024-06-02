using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.DataLoaders;

public class PortfolioItemByPortfolioCategoryIdGroupDataLoader(
    IDataRepository<IPortfolioItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, PortfolioItem>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, PortfolioItem>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            keys,
            x => x.CategoryId,
            PortfolioItemMappers.Map,
            cancellationToken
        );
}