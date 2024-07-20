using api.ContactItems.Interfaces;
using api.ContactItems.Models;

namespace api.ContactItems.Services;

public sealed class ContactItemDataRepository(ILogger<ContactItemDataRepository> logger, IOptionsSnapshot<ContactItemsDataConfig> dataSnapshot)
: InMemoryAbstractDataRepository<IContactItem, ContactItemsDataConfig>(logger, dataSnapshot)
{
    protected override IContactItem[]? ResolveSet(ContactItemsDataConfig data) => data.ContactItems;
}
