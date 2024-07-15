using api.ContactItems.Interfaces;
using api.ContactItems.Mappers;
using api.ContactItems.Models;

namespace api.ContactItems.Queries;

[QueryType]
public sealed class ContactItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<ContactItem> GetContactItems([Service] IDataRepository<IContactItem> repository) =>
        repository.Get(ContactItemMappers.Map);
}
