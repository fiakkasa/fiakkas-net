using api.Categories.DataLoaders;
using api.Categories.Interfaces;

namespace api.Categories.TypeExtensions;

[ExtendObjectType<ICategoryAssociatedCategoryTypes>]
public sealed class ICategoryAssociatedCategoryTypesTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IEnumerable<IPolymorphicCategory>> GetAssociatedCategories(
        [Parent] ICategoryAssociatedCategoryTypes parent,
        [Service] AssociatedCategoryGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        var result = await dataLoader.LoadAsync(parent.AssociatedCategoryTypes, cancellationToken);

        return result.SelectMany(x => x);
    }
}
