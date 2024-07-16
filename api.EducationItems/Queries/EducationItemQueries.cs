using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;

namespace api.EducationItems.Queries;

[QueryType]
public static class EducationItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<EducationItem> GetEducationItems([Service] IDataRepository<IEducationItem> repository) =>
        repository.Get(EducationItemMappers.Map);
}
