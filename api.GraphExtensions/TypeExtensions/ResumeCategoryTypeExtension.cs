using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<ResumeCategory>]
public sealed class ResumeCategoryTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<EducationItem[]> GetEducationItems(
        [Parent] ResumeCategory parent,
        [Service] EducationItemByResumeCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
