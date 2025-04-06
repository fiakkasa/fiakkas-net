namespace api.Shared.DataLoaders;

public abstract class AbstractGenericBatchDataLoaderById<TEntity, TMapped>(
    IDataRepository<TEntity> dataRepository,
    Func<TEntity, TMapped> mapper,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    Func<TEntity, IReadOnlyList<Guid>, bool>? predicate = default
) : BatchDataLoader<Guid, TMapped>(batchScheduler, options ?? new())
    where TEntity : IBaseId
    where TMapped : IBaseId
{
    protected override async Task<IReadOnlyDictionary<Guid, TMapped>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) => predicate switch
    {
        { } predicateFn => await dataRepository.GetBatch(
            x => predicateFn(x, keys),
            mapper,
            cancellationToken
        ),
        _ => await dataRepository.GetBatch(keys, mapper, cancellationToken)
    };
}
