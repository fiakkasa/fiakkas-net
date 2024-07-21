using api.Shared.Types.Interfaces;

namespace api.Shared.DataLoaders;

public abstract class AbstractGenericBatchDataLoaderById<TEntity, TMapped> : BatchDataLoader<Guid, TMapped>
where TEntity : IBaseId
where TMapped : IBaseId
{
    private readonly IDataRepository<TEntity> _dataRepository;
    private readonly Func<TEntity, TMapped> _mapper;
    private readonly Func<TEntity, IReadOnlyList<Guid>, bool>? _predicate;

    protected AbstractGenericBatchDataLoaderById(
        IDataRepository<TEntity> dataRepository,
        Func<TEntity, TMapped> mapper,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = default,
        Func<TEntity, IReadOnlyList<Guid>, bool>? predicate = default
    ) : base(batchScheduler, options)
    {
        _dataRepository = dataRepository;
        _mapper = mapper;
        _predicate = predicate;
    }

    protected override async Task<IReadOnlyDictionary<Guid, TMapped>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken
    ) => _predicate switch
    {
        { } predicate => await _dataRepository.GetBatch(
            x => predicate(x, keys),
            _mapper,
            cancellationToken
        ),
        _ => await _dataRepository.GetBatch(keys, _mapper, cancellationToken)
    };
}

