namespace api.Shared.Types.Interfaces;

public interface IDataRepository<TEntity> where TEntity : IBaseId
{
    IQueryable<TEntity> Get();
    IQueryable<TMapped> Get<TMapped>(Func<TEntity, TMapped> mapper);

    IQueryable<TMapped> Get<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper
    );

    ValueTask<TEntity?> Find(
        Guid id,
        CancellationToken cancellationToken = default
    );

    ValueTask<TEntity?> Find(
        Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default
    );

    ValueTask<TMapped?> Find<TMapped>(
        Guid id,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId;

    ValueTask<TMapped?> Find<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId;

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

    ValueTask<IReadOnlyDictionary<TKey, TMapped>> GetBatch<TMapped, TKey>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    )
        where TMapped : IBaseId
        where TKey : notnull;

    ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    );

    ValueTask<ILookup<TKey, TMapped>> GetGroupedBatch<TMapped, TKey>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;
}
