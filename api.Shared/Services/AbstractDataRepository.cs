using api.Shared.Interfaces;

namespace api.Shared.Services;

public abstract class AbstractDataRepository<TEntity, TConfig>(ILogger logger, IOptionsSnapshot<TConfig> dataSnapshot)
: IDataRepository<TEntity>
where TEntity : IBaseData
where TConfig : class
{
    private static readonly Type _type = typeof(TEntity);

    private T[] GetSet<T>() where T : IBaseData
    {
        var typeName = _type.FullName;

        if (!_type.IsInterface)
        {
            logger.LogWarning("Type {Type} is not supported", typeName);

            return [];
        }

        if (ResolveSet<T>(dataSnapshot.Value) is T[] collection)
            return collection;

        logger.LogWarning("Type {Type} is not supported", typeName);

        return [];
    }

    protected abstract T[]? ResolveSet<T>(TConfig data) where T : IBaseData;

    public IQueryable<TEntity> Get()
    {
        try
        {
            return GetSet<TEntity>().AsQueryable();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get data for type {Type}", typeof(TEntity).FullName);

            return Enumerable.Empty<TEntity>().AsQueryable();
        }
    }

    public IQueryable<TMapped> Get<TMapped>(Func<TEntity, TMapped> mapper) where TMapped : TEntity
    {
        try
        {
            return GetSet<TEntity>().Select(x => mapper(x)).AsQueryable();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get data for type {Type}", typeof(TEntity).FullName);

            return Enumerable.Empty<TMapped>().AsQueryable();
        }
    }

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken
    ) where TMapped : TEntity
    {
        try
        {
            return await Task.Run(() =>
                GetSet<TEntity>()
                    .Where(x => keys.Contains(x.Id))
                    .Select(mapper)
                    .ToDictionary(x => x.Id),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get batch data for type {Type}", typeof(TEntity).FullName);

            return new Dictionary<Guid, TMapped>();
        }
    }

    public async ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken
    ) where TMapped : TEntity
    {
        try
        {
            return await Task.Run(() =>
                GetSet<TEntity>()
                    .Where(x => keys.Contains(keySelector(x)))
                    .ToLookup(x => keySelector(x), x => mapper(x)),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get grouped batch data for type {Type}", typeof(TEntity).FullName);

            return Enumerable.Empty<TEntity>().ToLookup(x => x.Id, x => default(TMapped)!);
        }
    }
}
