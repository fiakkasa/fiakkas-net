using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

public sealed class ResumeCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : AbstractGenericBatchDataLoaderById<ICategory, ResumeCategory>(
    dataRepository,
    CategoryMappers.MapResumeCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsResumeCategory(x)
        && keys.Contains(x.Id)
);
