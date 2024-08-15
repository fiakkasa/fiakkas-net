using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Services;

public sealed class CategoryDataRepository(
    ILogger<CategoryDataRepository> logger,
    IOptionsSnapshot<CategoriesDataConfig> dataSnapshot
)
    : AbstractInMemoryDataRepository<ICategory, CategoriesDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<ICategory>? ResolveSet(CategoriesDataConfig data) => data.Categories;
}
