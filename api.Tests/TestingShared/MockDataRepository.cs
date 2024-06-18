using api.Shared.Interfaces;

namespace api.Tests.TestingShared;

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

    public ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        Func<T, bool> predicate,
        Func<T, Guid> keySelector,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) =>
        ValueTask.FromResult(
            _collection
                .Where(predicate)
                .ToLookup(keySelector, mapper)
        );
}
