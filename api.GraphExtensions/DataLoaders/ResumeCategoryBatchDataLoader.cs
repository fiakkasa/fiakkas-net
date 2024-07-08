namespace api.GraphExtensions.DataLoaders;

public sealed class ResumeCategoryBatchDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : BatchDataLoader<Guid, ResumeCategory>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, ResumeCategory>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetBatch(
            x =>
                CategoryEntityUtils.IsResumeCategory(x)
                && keys.Contains(x.Id),
            CategoryMappers.MapGenericCategory<ResumeCategory>,
            cancellationToken
        );
}
