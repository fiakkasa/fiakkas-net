using api.Shared.Interfaces;

namespace api.GraphExtensions.TestingShared;

public class MockDataRepository<T>(T[]? collection = default) : IDataRepository<T> where T : IBaseId
{
    private readonly T[] _collection = collection ?? [];

    public IQueryable<T> Get() => _collection.AsQueryable<T>();

    public IQueryable<TMapped> Get<TMapped>(Func<T, TMapped> mapper)
        => _collection.Select(mapper).AsQueryable();

    public async ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) where TMapped : IBaseId
    =>
        await ValueTask.FromResult(
            _collection.ToDictionary(key => key.Id, value => mapper(value))
        );

    public ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
        IReadOnlyList<Guid> keys,
        Func<T, Guid> keySelector,
        Func<T, TMapped> mapper,
        CancellationToken cancellationToken = default
    ) =>
        ValueTask.FromResult(
            _collection.ToLookup(x => keySelector(x), x => mapper(x))
        );
}
