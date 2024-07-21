using api.Categories.DataLoaders;
using api.Categories.Models;

namespace api.Categories.TypeExtensions;

[ExtendObjectType<ResumeCategory>]
public sealed class ResumeCategoryTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IEnumerable<Category>> GetAssociatedCategories(
        [Parent] ResumeCategory parent,
        [Service] AssociatedCategoryGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        var result = await dataLoader.LoadAsync(parent.AssociatedCategoryTypes, cancellationToken);

        return result.SelectMany(x => x);
    }
}
