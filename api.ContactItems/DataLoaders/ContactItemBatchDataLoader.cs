using api.ContactItems.Interfaces;
using api.ContactItems.Mappers;
using api.ContactItems.Models;

namespace api.ContactItems.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class ContactItemBatchDataLoader(
    IDataRepository<IContactItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GenericBatchDataLoaderById<IContactItem, ContactItem>(
    dataRepository,
    ContactItemMappers.Map,
    batchScheduler,
    options
)
{ }
