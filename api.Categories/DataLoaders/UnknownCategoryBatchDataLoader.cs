using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class UnknownCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, UnknownCategory>(
    dataRepository,
    CategoryMappers.MapUnknownCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsUnknownCategory(x)
        && keys.Contains(x.Id)
);
