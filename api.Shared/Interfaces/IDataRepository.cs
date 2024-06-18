namespace api.Shared.Interfaces;

public interface IDataRepository<TEntity> where TEntity : IBaseId
{
    IQueryable<TEntity> Get();
    IQueryable<TMapped> Get<TMapped>(Func<TEntity, TMapped> mapper);
    IQueryable<TMapped> Get<TMapped>(Func<TEntity, bool> predicate, Func<TEntity, TMapped> mapper);

    ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId;

    ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId;

    ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    );

    ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    );
}
