using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class PortfolioItemBatchDataLoader(
    IDataRepository<IPortfolioItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<IPortfolioItem, PortfolioItem>(
    dataRepository,
    PortfolioItemMappers.Map,
    batchScheduler,
    options
);
