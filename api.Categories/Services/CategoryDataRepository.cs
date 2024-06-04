using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Services;

public class CategoryDataRepository(ILogger<CategoryDataRepository> logger, IOptionsSnapshot<CategoriesDataConfig> dataSnapshot)
: AbstractDataRepository<ICategory, CategoriesDataConfig>(logger, dataSnapshot)
{
    protected override ICategory[]? ResolveSet(CategoriesDataConfig data) => data.Categories;
}
