using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class OtherCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : AbstractGenericBatchDataLoaderById<ICategory, OtherCategory>(
    dataRepository,
    CategoryMappers.MapOtherCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsOtherCategory(x)
        && keys.Contains(x.Id)
);
