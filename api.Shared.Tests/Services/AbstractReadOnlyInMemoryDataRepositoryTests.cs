using api.Shared.Services;
using api.Shared.Types.Interfaces;

namespace api.Shared.Tests.Services;

public class AbstractReadOnlyInMemoryDataRepositoryTests
{
    private readonly ILogger<TestDataRepository> _logger;
    private readonly ILogger<TestDataRepositoryWrongType> _loggerWrongType;
    private readonly IOptionsSnapshot<TestConfig> _optionsSnapshot;
    private readonly TestDataRepository _sut;
    private readonly TestDataRepositoryWrongType _sutWrongType;

    public AbstractReadOnlyInMemoryDataRepositoryTests()
    {
        _logger = Substitute.For<ILogger<TestDataRepository>>();
        _loggerWrongType = Substitute.For<ILogger<TestDataRepositoryWrongType>>();
        _optionsSnapshot = Substitute.For<IOptionsSnapshot<TestConfig>>();
        _sut = new(_logger, _optionsSnapshot);
        _sutWrongType = new(_loggerWrongType, _optionsSnapshot);
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sutWrongType.Get().ToArray();

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sut.Get().ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = _sut.Get().ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = new("99e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]));

        var result = _sut.Get().ToArray();

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sutWrongType.Get(x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sut.Get(x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = _sut.Get(x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = new("99e483e4-6961-4b25-88a9-d1d0a5161109")
            }
        ]));

        var result = _sut.Get(x => x.Id).ToArray();

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sutWrongType.Get(_ => true, x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = _sut.Get(_ => true, x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = _sut.Get(_ => true, x => x.Id).ToArray();

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = _sut.Get(x => x.Id == id, x => x.Id).ToArray();

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task Find_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.Find(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
    }

    [Fact]
    public async Task Find_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.Find(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
    }

    [Fact]
    public async Task Find_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.Find(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get item with id {Id} for type {Type}"
            }
        );
    }

    [Fact]
    public async Task Find_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.Find(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task FindPredicate_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.Find(_ => true, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
    }

    [Fact]
    public async Task FindPredicate_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.Find(_ => true, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
    }

    [Fact]
    public async Task FindPredicate_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.Find(_ => true, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get item for type {Type}"
            }
        );
    }

    [Fact]
    public async Task FindPredicate_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.Find(x => x.Id == id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task FindMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.Find(Guid.NewGuid(), x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
    }

    [Fact]
    public async Task FindMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.Find(Guid.NewGuid(), x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
    }

    [Fact]
    public async Task FindMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.Find(Guid.NewGuid(), x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get item with id {Id} for type {Type} and mapped type {MappedType}"
            }
        );
    }

    [Fact]
    public async Task FindMapped_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.Find(id, x => x, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task FindPredicateMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.Find(_ => true, x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
    }

    [Fact]
    public async Task FindPredicateMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.Find(_ => true, x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
    }

    [Fact]
    public async Task FindPredicateMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.Find(_ => true, x => x, CancellationToken.None);

        Assert.Null(result);
        Assert.Single(
             _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get item for type {Type} and mapped type {MappedType}"
            }
        );
    }

    [Fact]
    public async Task FindPredicateMapped_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.Find(x => x.Id == id, x => x, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.GetBatch([], x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.GetBatch([id], x => x, CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.GetBatch(_ => true, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.GetBatch(_ => true, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.GetBatch(_ => true, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.GetBatch(x => x.Id == id, x => x, CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.GetBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.GetBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.GetBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.GetBatch(x => x.Id == id, x => x.Id, x => x, CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get grouped batch data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.GetGroupedBatch([id], x => x.Id, x => x, CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sutWrongType.GetGroupedBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _loggerWrongType.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(_ => new());

        var result = await _sut.GetGroupedBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(_ => throw new("Splash!"));

        var result = await _sut.GetGroupedBatch(_ => true, x => x.Id, x => x, CancellationToken.None);

        Assert.Empty(result);
        Assert.Single(
            _logger.GetLogsResultsCollection(),
            x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get grouped batch data for type {Type} and mapped type {MappedType}"
            }
        );
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(_ => new([
            new()
            {
                Id = id
            }
        ]));

        var result = await _sut.GetGroupedBatch(x => x.Id == id, x => x.Id, x => x, CancellationToken.None);

        Assert.Single(result);
        Assert.Empty(_logger.ReceivedCalls());
        result.MatchSnapshot();
    }

    public interface ITestEntity : IBaseId;

    public record TestEntity : ITestEntity
    {
        public Guid Id { get; init; }
    }

    public record TestConfig(TestEntity[]? Collection = default);

    public class TestDataRepositoryWrongType(
        ILogger<TestDataRepositoryWrongType> logger,
        IOptionsSnapshot<TestConfig> dataSnapshot
    )
        : AbstractReadOnlyInMemoryDataRepository<TestEntity, TestConfig>(logger, dataSnapshot)
    {
        protected override TestEntity[]? ResolveSet(TestConfig data) => data.Collection;
    }

    public class TestDataRepository(ILogger<TestDataRepository> logger, IOptionsSnapshot<TestConfig> dataSnapshot)
        : AbstractReadOnlyInMemoryDataRepository<ITestEntity, TestConfig>(logger, dataSnapshot)
    {
        protected override IReadOnlyCollection<ITestEntity>? ResolveSet(TestConfig data) => data.Collection;
    }
}
