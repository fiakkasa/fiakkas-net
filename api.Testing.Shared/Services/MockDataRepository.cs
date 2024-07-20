using api.Shared.Types.Interfaces;

namespace api.Testing.Shared.Services;

[ExcludeFromCodeCoverage]
public class MockDataRepository<T>(T[]? collection = default) : IDataRepository<T> where T : IBaseId
{
    private readonly T[] _collection = collection ?? [];

    public IQueryable<T> Get() => _collection.AsQueryable();

    public IQueryable<TMapped> Get<TMapped>(Func<T, TMapped> mapper) =>
        _collection
            .Select(mapper)
            .AsQueryable();

    public IQueryable<TMapped> Get<TMapped>(Func<T, bool> predicate, Func<T, TMapped> mapper) =>
        _collection
            .Where(predicate)
            .Select(mapper)
            .AsQueryable();

    public ValueTask<T?> Find(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        ValueTask.FromResult(Array.Find(_collection, x => x.Id == id));

    public ValueTask<T?> Find(
        Func<T, bool> predicate,
        CancellationToken cancellationToken = default
    ) =>
        ValueTask.FromResult(Array.Find(_collection, x => predicate(x)));

    public ValueTask<TMapped?> Find<TMapped>(
        Guid id,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    =>
        ValueTask.FromResult(
            Array.Find(_collection, x => x.Id == id) switch
            {
                { } result => mapper(result),
                _ => default
            }
        );

    public ValueTask<TMapped?> Find<TMapped>(
        Func<T, bool> predicate,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    =>
        ValueTask.FromResult(
            Array.Find(_collection, x => predicate(x)) switch
            {
                { } result => mapper(result),
                _ => default
            }
        );

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    =>
        await ValueTask.FromResult(
            _collection
                .Where(x => keys.Contains(x.Id))
                .Select(mapper)
                .ToDictionary(x => x.Id)
        );

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        Func<T, bool> predicate,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    =>
        await ValueTask.FromResult(
            _collection
                .Where(predicate)
                .ToDictionary(x => x.Id, mapper)
        );

    public async ValueTask<IReadOnlyDictionary<TKey, TMapped>> GetBatch<TMapped, TKey>(
        Func<T, bool> predicate,
        Func<T, TKey> keySelector,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    )
    where TMapped : IBaseId
    where TKey : notnull
    =>
        await ValueTask.FromResult(
            _collection
                .Where(predicate)
                .ToDictionary(x => keySelector(x), mapper)
        );

    public ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<T, Guid> keySelector,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) =>
        ValueTask.FromResult(
            _collection
                .Where(x => keys.Contains(keySelector(x)))
                .ToLookup(keySelector, mapper)
        );

    public ValueTask<ILookup<TKey, TMapped>> GetGroupedBatch<TMapped, TKey>(
        Func<T, bool> predicate,
        Func<T, TKey> keySelector,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TKey : notnull
     =>
        ValueTask.FromResult(
            _collection
                .Where(predicate)
                .ToLookup(keySelector, mapper)
        );
}
