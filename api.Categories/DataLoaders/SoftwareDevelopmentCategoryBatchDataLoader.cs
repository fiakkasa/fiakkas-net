using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class SoftwareDevelopmentCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, SoftwareDevelopmentCategory>(
    dataRepository,
    CategoryMappers.MapSoftwareDevelopmentCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsSoftwareDevelopmentCategory(x)
        && keys.Contains(x.Id)
);
