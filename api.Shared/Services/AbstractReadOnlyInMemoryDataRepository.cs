using api.Shared.Types.Interfaces;

namespace api.Shared.Services;

public abstract class AbstractReadOnlyInMemoryDataRepository<TEntity, TConfig>(
    ILogger logger,
    IOptionsSnapshot<TConfig> dataSnapshot
)
    : IDataRepository<TEntity>
    where TEntity : IBaseId
    where TConfig : class
{
    private static readonly Type _type = typeof(TEntity);

    public IQueryable<TEntity> Get()
    {
        try
        {
            return GetSet().AsQueryable();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get data for type {Type}", _type.Name);

            return Enumerable.Empty<TEntity>().AsQueryable();
        }
    }

    public IQueryable<TMapped> Get<TMapped>(Func<TEntity, TMapped> mapper)
    {
        try
        {
            return GetSet()
                .Select(mapper)
                .AsQueryable();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return Enumerable.Empty<TMapped>().AsQueryable();
        }
    }

    public IQueryable<TMapped> Get<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper
    )
    {
        try
        {
            return GetSet()
                .Where(predicate)
                .Select(mapper)
                .AsQueryable();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return Enumerable.Empty<TMapped>().AsQueryable();
        }
    }

    public async ValueTask<TEntity?> Find(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await Task.Run(() =>
                    Array.Find(GetSet(), x => x.Id == id),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get item with id {Id} for type {Type}",
                id,
                _type.Name
            );

            return default;
        }
    }

    public async ValueTask<TEntity?> Find(
        Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await Task.Run(() =>
                    Array.Find(GetSet(), x => predicate(x)),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get item for type {Type}",
                _type.Name
            );

            return default;
        }
    }

    public async ValueTask<TMapped?> Find<TMapped>(
        Guid id,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    {
        try
        {
            return await Task.Run(() =>
                    Array.Find(GetSet(), x => x.Id == id) switch
                    {
                        { } result => mapper(result),
                        _ => default
                    },
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get item with id {Id} for type {Type} and mapped type {MappedType}",
                id,
                _type.Name,
                typeof(TMapped).Name
            );

            return default;
        }
    }

    public async ValueTask<TMapped?> Find<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    {
        try
        {
            return await Task.Run(() =>
                    Array.Find(GetSet(), x => predicate(x)) switch
                    {
                        { } result => mapper(result),
                        _ => default
                    },
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get item for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return default;
        }
    }

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    {
        try
        {
            return await Task.Run(() =>
                    GetSet()
                        .Where(x => keys.Contains(x.Id))
                        .ToDictionary(x => x.Id, mapper),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get batch data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return new Dictionary<Guid, TMapped>();
        }
    }

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    {
        try
        {
            return await Task.Run(() =>
                    GetSet()
                        .Where(predicate)
                        .ToDictionary(x => x.Id, mapper),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get batch data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return new Dictionary<Guid, TMapped>();
        }
    }

    public async ValueTask<IReadOnlyDictionary<TKey, TMapped>> GetBatch<TMapped, TKey>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    )
        where TMapped : IBaseId
        where TKey : notnull
    {
        try
        {
            return await Task.Run(() =>
                    GetSet()
                        .Where(predicate)
                        .ToDictionary(keySelector, mapper),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get batch data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return new Dictionary<TKey, TMapped>();
        }
    }

    public async ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<TEntity, Guid> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await Task.Run(() =>
                    GetSet()
                        .Where(x => keys.Contains(keySelector(x)))
                        .ToLookup(keySelector, mapper),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get grouped batch data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return Enumerable.Empty<TEntity>().ToLookup(x => x.Id, _ => default(TMapped)!);
        }
    }

    public async ValueTask<ILookup<TKey, TMapped>> GetGroupedBatch<TMapped, TKey>(
        Func<TEntity, bool> predicate,
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TKey : notnull
    {
        try
        {
            return await Task.Run(() =>
                    GetSet()
                        .Where(predicate)
                        .ToLookup(keySelector, mapper),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get grouped batch data for type {Type} and mapped type {MappedType}",
                _type.Name,
                typeof(TMapped).Name
            );

            return Enumerable.Empty<TEntity>().ToLookup(_ => default(TKey)!, _ => default(TMapped)!);
        }
    }

    private TEntity[] GetSet()
    {
        if (!_type.IsInterface)
        {
            logger.LogWarning("Type {Type} is not supported", _type.Name);

            return [];
        }

        if (ResolveSet(dataSnapshot.Value) is TEntity[] collection)
        {
            return collection;
        }

        logger.LogWarning("Resolver for type {Type} could not materialize collection", _type.Name);

        return [];
    }

    protected abstract IReadOnlyCollection<TEntity>? ResolveSet(TConfig data);
}
