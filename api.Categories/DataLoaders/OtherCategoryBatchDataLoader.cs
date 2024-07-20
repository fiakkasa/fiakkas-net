using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class OtherCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GenericBatchDataLoaderById<ICategoryEntity, OtherCategory>(
    dataRepository,
    CategoryMappers.MapGenericCategory<OtherCategory>,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsOtherCategory(x)
        && keys.Contains(x.Id)
)
{ }
