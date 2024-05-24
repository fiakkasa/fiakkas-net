using api.Shared.Interfaces;
using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.GraphExtensions.DataLoaders;

public class TechnologyBatchDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<ITechnology>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<ITechnology, Technology>>(), Arg.Any<CancellationToken>())
            .Returns(
                new[]
                {
                    new Technology
                    {
                        Id = Guid.Empty,
                        CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        UpdatedAt = null,
                        Version = 1,
                        Title = "Title",
                        Href = new Uri("/test", UriKind.Relative)
                    }
                }
                .ToDictionary(x => x.Id)
            );

        var sut = new TechnologyBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<ITechnology>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<ITechnology, Technology>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, Technology>());

        var sut = new TechnologyBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
