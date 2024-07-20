using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class SoftwareDevelopmentCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategoryEntity, SoftwareDevelopmentCategory>(
    dataRepository,
    CategoryMappers.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsSoftwareDevelopmentCategory(x)
        && keys.Contains(x.Id)
)
{ }
