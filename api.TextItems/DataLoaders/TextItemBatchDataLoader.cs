using api.TextItems.Interfaces;
using api.TextItems.Mappers;
using api.TextItems.Models;

namespace api.TextItems.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class TextItemBatchDataLoader(
    IDataRepository<ITextItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : AbstractGenericBatchDataLoaderById<ITextItem, TextItem>(
    dataRepository,
    TextItemMappers.Map,
    batchScheduler,
    options
);
