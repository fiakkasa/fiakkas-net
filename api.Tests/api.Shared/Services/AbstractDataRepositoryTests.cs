using api.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Shared.Services.Tests;

public class AbstractDataRepositoryTests
{
    public interface ITestEntity : IBaseId { }

    public record TestEntity : ITestEntity
    {
        public Guid Id { get; init; }
    }

    public record TestConfig(TestEntity[]? Collection = default);

    public class TestDataRepositoryWrongType(ILogger<TestDataRepositoryWrongType> logger, IOptionsSnapshot<TestConfig> dataSnapshot)
    : AbstractDataRepository<TestEntity, TestConfig>(logger, dataSnapshot)
    {
        protected override TestEntity[]? ResolveSet(TestConfig data) => data.Collection;
    }

    public class TestDataRepository(ILogger<TestDataRepository> logger, IOptionsSnapshot<TestConfig> dataSnapshot)
    : AbstractDataRepository<ITestEntity, TestConfig>(logger, dataSnapshot)
    {
        protected override ITestEntity[]? ResolveSet(TestConfig data) => data.Collection;
    }

    private readonly ILogger<TestDataRepository> _logger;
    private readonly ILogger<TestDataRepositoryWrongType> _loggerWrongType;
    private readonly IOptionsSnapshot<TestConfig> _optionsSnapshot;
    private readonly TestDataRepository _sut;
    private readonly TestDataRepositoryWrongType _sutWrongType;

    public AbstractDataRepositoryTests()
    {
        _logger = Substitute.For<ILogger<TestDataRepository>>();
        _loggerWrongType = Substitute.For<ILogger<TestDataRepositoryWrongType>>();
        _optionsSnapshot = Substitute.For<IOptionsSnapshot<TestConfig>>();
        _sut = new TestDataRepository(_logger, _optionsSnapshot);
        _sutWrongType = new TestDataRepositoryWrongType(_loggerWrongType, _optionsSnapshot);
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sutWrongType.Get().ToArray();

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sut.Get().ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = _sut.Get().ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void Get_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109") }]));

        var result = _sut.Get().ToArray();

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sutWrongType.Get(x => x.Id).ToArray();

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sut.Get(x => x.Id).ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = _sut.Get(x => x.Id).ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetMapped_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109") }]));

        var result = _sut.Get(x => x.Id).ToArray();

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sutWrongType.Get(x => true, x => x.Id).ToArray();

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sut.Get(x => true, x => x.Id).ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = _sut.Get(x => true, x => x.Id).ToArray();

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPredicateMapped_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = _sut.Get(x => x.Id == id, x => x.Id).ToArray();

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatch_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetBatch([id], x => x, CancellationToken.None);

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetBatch(x => true, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetBatch(x => true, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetBatch(x => true, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicate_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetBatch(x => x.Id == id, x => x, CancellationToken.None);

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get batch data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetBatchPredicateKeySelector_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetBatch(x => x.Id == id, x => x.Id, x => x, CancellationToken.None);

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get grouped batch data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetGroupedBatch([id], x => x.Id, x => x, CancellationToken.None);

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Non_Interface_Type_Used()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetGroupedBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Type {Type} is not supported"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Null_Data()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetGroupedBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Warning,
                OriginalMessage: "Resolver for type {Type} could not materialize collection"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Empty_Collection_When_Exception_Occurs()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetGroupedBatch(x => true, x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .GetLogsResultsCollection()
            .Where(x => x is
            {
                LogLevel: LogLevel.Error,
                OriginalMessage: "Failed to get grouped batch data for type {Type} and mapped type {MappedType}"
            })
            .Should()
            .ContainSingle();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetGroupedBatchPredicate_Should_Return_Collection()
    {
        var id = new Guid("99e483e4-6961-4b25-88a9-d1d0a5161109");
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetGroupedBatch(x => x.Id == id, x => x.Id, x => x, CancellationToken.None);

        result.Should().ContainSingle();
        _logger.ReceivedCalls().Should().BeEmpty();
        result.MatchSnapshot();
    }
}

