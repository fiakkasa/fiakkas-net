using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class PortfolioCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, PortfolioCategory>(
    dataRepository,
    CategoryMappers.MapPortfolioCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsPortfolioCategory(x)
        && keys.Contains(x.Id)
)
{ }
