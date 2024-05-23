using api.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Shared.Services.Tests;

public class AbstractDataRepositoryTests
{
    public interface ITestEntity : IBaseData { }

    public record TestEntity : ITestEntity
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
    }

    public record TestConfig(TestEntity[]? Collection = default);

    public class TestDataRepositoryWrongType(ILogger<TestDataRepositoryWrongType> logger, IOptionsSnapshot<TestConfig> dataSnapshot)
    : AbstractDataRepository<TestEntity, TestConfig>(logger, dataSnapshot)
    {
        public virtual TestEntity[]? Collection { get; set; }

        protected override TestEntity[]? ResolveSet(TestConfig data) => Collection;
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
    public void Get_On_Non_Interface_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sutWrongType.Get();

        result.Should().BeEmpty();
        _loggerWrongType
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Type {Type} is not supported" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void Get_On_Null_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sut.Get();

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Resolver for type {Type} could not materialize collection" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void Get_On_Exception_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = _sut.Get();

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Error, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Failed to get data for type {Type}" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void Get_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new()]));

        var result = _sut.Get();

        result.Should().NotBeEmpty();
        _logger.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void GetMapped_On_Non_Interface_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sutWrongType.Get(x => x.Id);

        result.Should().BeEmpty();
        _loggerWrongType
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Type {Type} is not supported" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void GetMapped_On_Null_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = _sut.Get(x => x.Id);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Resolver for type {Type} could not materialize collection" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void GetMapped_On_Exception_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = _sut.Get(x => x.Id);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
               x.GetOriginalArguments() is [LogLevel.Error, _, IEnumerable<KeyValuePair<string, object>> items, ..]
               && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Failed to get data for type {Type} and mapped type {MappedType}" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void GetMapped_Should_Return_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new()]));

        var result = _sut.Get(x => x.Id);

        result.Should().NotBeEmpty();
        _logger.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task GetBatch_On_Non_Interface_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Type {Type} is not supported" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetBatch_On_Null_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Resolver for type {Type} could not materialize collection" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetBatch_On_Exception_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetBatch([], x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
           .Where(x =>
               x.GetOriginalArguments() is [LogLevel.Error, _, IEnumerable<KeyValuePair<string, object>> items, ..]
               && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Failed to get batch data for type {Type} and mapped type {MappedType}" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetBatch_Should_Return_Collection()
    {
        var id = Guid.NewGuid();
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetBatch([id], x => x, CancellationToken.None);

        result.Should().NotBeEmpty();
        _logger.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task GetGroupedBatch_On_Non_Interface_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sutWrongType.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _loggerWrongType
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Type {Type} is not supported" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetGroupedBatch_On_Null_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => new TestConfig());

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
            .Where(x =>
                x.GetOriginalArguments() is [LogLevel.Warning, _, IEnumerable<KeyValuePair<string, object>> items, ..]
                && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Resolver for type {Type} could not materialize collection" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetGroupedBatch_On_Exception_Should_Return_Empty_Collection()
    {
        _optionsSnapshot.Value.Returns(x => throw new Exception("Splash!"));

        var result = await _sut.GetGroupedBatch([], x => x.Id, x => x, CancellationToken.None);

        result.Should().BeEmpty();
        _logger
            .ReceivedCalls()
           .Where(x =>
               x.GetOriginalArguments() is [LogLevel.Error, _, IEnumerable<KeyValuePair<string, object>> items, ..]
               && items.Any(y => y is { Key: "{OriginalFormat}", Value: "Failed to get grouped batch data for type {Type} and mapped type {MappedType}" })
            )
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public async Task GetGroupedBatch_Should_Return_Collection()
    {
        var id = Guid.NewGuid();
        _optionsSnapshot.Value.Returns(x => new TestConfig(Collection: [new() { Id = id }]));

        var result = await _sut.GetGroupedBatch([id], x => x.Id, x => x, CancellationToken.None);

        result.Should().NotBeEmpty();
        _logger.ReceivedCalls().Should().BeEmpty();
    }
}

