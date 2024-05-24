using api.Shared.Interfaces;

namespace api.Shared.Services;

public abstract class AbstractDataRepository<TEntity, TConfig>(ILogger logger, IOptionsSnapshot<TConfig> dataSnapshot)
: IDataRepository<TEntity>
where TEntity : IBaseId
where TConfig : class
{
    private static readonly Type _type = typeof(TEntity);

    private TEntity[] GetSet()
    {
        if (!_type.IsInterface)
        {
            logger.LogWarning("Type {Type} is not supported", _type.Name);

            return [];
        }

        if (ResolveSet(dataSnapshot.Value) is TEntity[] collection)
            return collection;

        logger.LogWarning("Resolver for type {Type} could not materialize collection", _type.Name);

        return [];
    }

    protected abstract TEntity[]? ResolveSet(TConfig data);

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
            return GetSet().Select(x => mapper(x)).AsQueryable();
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
                    .Select(mapper)
                    .ToDictionary(x => x.Id),
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
                    .ToLookup(x => keySelector(x), x => mapper(x)),
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

            return Enumerable.Empty<TEntity>().ToLookup(x => x.Id, x => default(TMapped)!);
        }
    }
}
