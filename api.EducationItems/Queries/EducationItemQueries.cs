using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;

namespace api.EducationItems.Queries;

[QueryType]
public sealed class EducationItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<EducationItem> GetEducationItems([Service] IDataRepository<IEducationItem> repository) =>
        repository.Get(EducationItemMappers.Map);
}
