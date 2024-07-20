using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Services;

public sealed class CategoryDataRepository(ILogger<CategoryDataRepository> logger, IOptionsSnapshot<CategoriesDataConfig> dataSnapshot)
: AbstractInMemoryDataRepository<ICategoryEntity, CategoriesDataConfig>(logger, dataSnapshot)
{
    protected override ICategoryEntity[]? ResolveSet(CategoriesDataConfig data) => data.Categories;
}
