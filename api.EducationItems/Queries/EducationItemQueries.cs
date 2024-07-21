using api.EducationItems.DataLoaders;
using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;

namespace api.EducationItems.Queries;

[QueryType]
public static class EducationItemQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<EducationItem> GetEducationItems([Service] IDataRepository<IEducationItem> repository) =>
        repository.Get(EducationItemMappers.Map);

    [NodeResolver]
    public static async ValueTask<EducationItem?> GetEducationItemById(
        Guid id,
        EducationItemBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
