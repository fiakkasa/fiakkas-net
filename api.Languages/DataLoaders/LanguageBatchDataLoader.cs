using api.Languages.Interfaces;
using api.Languages.Mappers;
using api.Languages.Models;

namespace api.Languages.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class LanguageBatchDataLoader(
    IDataRepository<ILanguage> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GenericBatchDataLoaderById<ILanguage, Language>(
    dataRepository,
    LanguageMappers.Map,
    batchScheduler,
    options
)
{ }
