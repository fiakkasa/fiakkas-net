using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;

namespace api.EducationItems.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class EducationItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<EducationItem> GetEducationItems([Service] IDataRepository<IEducationItem<EducationTimePeriod>> repository) =>
        repository.Get(EducationItemMappers.Map);
}
