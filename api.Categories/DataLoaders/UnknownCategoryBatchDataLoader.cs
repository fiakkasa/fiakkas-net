using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class UnknownCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategoryEntity, UnknownCategory>(
    dataRepository,
    CategoryMappers.MapGenericCategory<UnknownCategory>,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsUnknownCategory(x)
        && keys.Contains(x.Id)
)
{ }
