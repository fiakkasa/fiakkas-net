using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;

namespace api.Categories.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class CategoryBatchDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<ICategory, Category>(
    dataRepository,
    CategoryMappers.MapCategory,
    batchScheduler,
    options
)
{ }
