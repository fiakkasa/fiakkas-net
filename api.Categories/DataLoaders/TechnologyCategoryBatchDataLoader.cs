using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class TechnologyCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, TechnologyCategory>(
    dataRepository,
    CategoryMappers.MapTechnologyCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsTechnologyCategory(x)
        && keys.Contains(x.Id)
)
{ }
