using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;

namespace api.EducationItems.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class EducationItemBatchDataLoader(
    IDataRepository<IEducationItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : AbstractGenericBatchDataLoaderById<IEducationItem, EducationItem>(
    dataRepository,
    EducationItemMappers.Map,
    batchScheduler,
    options
)
{ }
