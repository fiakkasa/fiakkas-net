using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.DataLoaders;

public class PortfolioCategoryBatchDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<IPortfolioCategory>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>(), Arg.Any<CancellationToken>())
            .Returns(
                new[]
                {
                    new PortfolioCategory
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

        var sut = new PortfolioCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<IPortfolioCategory>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, PortfolioCategory>());

        var sut = new PortfolioCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
