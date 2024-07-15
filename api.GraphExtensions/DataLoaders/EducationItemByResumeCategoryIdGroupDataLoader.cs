namespace api.GraphExtensions.DataLoaders;

public sealed class EducationItemByResumeCategoryIdGroupDataLoader(
    IDataRepository<IEducationItem> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<Guid, EducationItem>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EducationItem>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            keys,
            x => x.CategoryId,
            EducationItemMappers.Map,
            cancellationToken
        );
}
