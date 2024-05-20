namespace api.Shared.Interfaces;

public interface IDataRepository<TEntity> where TEntity : IBaseData
{
    IQueryable<TEntity> Get();
    IQueryable<TMapped> Get<TMapped>(Func<TEntity, TMapped> mapper) where TMapped : TEntity;

    ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken
    ) where TMapped : TEntity;

    ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken
    ) where TMapped : TEntity;
}
