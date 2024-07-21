using api.ContactItems.DataLoaders;
using api.ContactItems.Interfaces;
using api.ContactItems.Mappers;
using api.ContactItems.Models;

namespace api.ContactItems.Queries;

[QueryType]
public static class ContactItemQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<ContactItem> GetContactItems([Service] IDataRepository<IContactItem> repository) =>
        repository.Get(ContactItemMappers.Map);

    [NodeResolver]
    public static async ValueTask<ContactItem?> GetContactItemById(
        Guid id,
        ContactItemBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
