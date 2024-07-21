using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class ResumeCategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, ResumeCategory>(
    dataRepository,
    CategoryMappers.MapResumeCategory,
    batchScheduler,
    options,
    (x, keys) =>
        CategoryEntityUtils.IsResumeCategory(x)
        && keys.Contains(x.Id)
)
{ }
