using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class InformationTechnologyCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategoryEntity, InformationTechnologyCategory>(
    dataRepository,
    CategoryMappers.MapGenericTechnologyCategory<InformationTechnologyCategory>,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsInformationTechnologyCategory(x)
        && keys.Contains(x.Id)
)
{ }
