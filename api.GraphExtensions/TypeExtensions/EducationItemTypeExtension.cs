namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<EducationItem>]
public sealed class EducationItemTypeExtension
{
    public async ValueTask<ResumeCategory?> GetCategory(
        [Parent] EducationItem parent,
        [Service] ResumeCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CategoryId, cancellationToken);
}
