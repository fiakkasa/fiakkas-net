using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class InformationTechnologyCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, InformationTechnologyCategory>(
    dataRepository,
    CategoryMappers.MapInformationTechnologyCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsInformationTechnologyCategory(x)
        && keys.Contains(x.Id)
);
