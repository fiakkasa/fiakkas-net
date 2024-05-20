using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.DataLoaders;

public class PortfolioCategoryBatchDataLoader(
    IDataRepository<IPortfolioCategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, PortfolioCategory>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, PortfolioCategory>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(keys, PortfolioCategoryMappers.Map, cancellationToken);
}
