using api.ContactItems.Interfaces;
using api.ContactItems.Models;

namespace api.ContactItems.Services;

public sealed class ContactItemDataRepository(
    ILogger<ContactItemDataRepository> logger,
    IOptionsSnapshot<ContactItemsDataConfig> dataSnapshot
) : AbstractReadOnlyInMemoryDataRepository<IContactItem, ContactItemsDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<IContactItem>? ResolveSet(ContactItemsDataConfig data) => data.ContactItems;
}
