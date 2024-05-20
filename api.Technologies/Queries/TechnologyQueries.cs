using api.Technologies.Interfaces;
using api.Technologies.Mappers;
using api.Technologies.Models;

namespace api.Technologies.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class TechnologyQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Technology> GetTechnologies([Service] IDataRepository<ITechnology> repository) =>
        repository.Get(TechnologyMappers.Map);
}
