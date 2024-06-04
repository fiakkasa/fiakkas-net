using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;

namespace api.Categories.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class CategoryQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Category> GetCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(CategoryMappers.Map);
}
