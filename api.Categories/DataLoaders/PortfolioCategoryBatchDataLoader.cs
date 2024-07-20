using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class PortfolioCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GenericBatchDataLoaderById<ICategoryEntity, PortfolioCategory>(
    dataRepository,
    CategoryMappers.MapGenericCategory<PortfolioCategory>,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsPortfolioCategory(x)
        && keys.Contains(x.Id)
)
{ }
