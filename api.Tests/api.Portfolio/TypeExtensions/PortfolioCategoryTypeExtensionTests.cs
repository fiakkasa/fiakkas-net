using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.TypeExtensions;

public class PortfolioCategoryTypeExtensionTests
{
    [Fact]
    public async Task GetPortfolioItems_Should_Return_Data()
    {
        var items = new[]
        {
            new PortfolioItem
            {
                Id = Guid.Empty,
                CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative),
                TechnologyIds = [Guid.Empty],
                CustomerId = Guid.Empty
            }
        };
        var dataRepository = Substitute.For<IDataRepository<IPortfolioItem>>();
        dataRepository
            .GetGroupedBatch(
                Arg.Any<IReadOnlyList<Guid>>(),
                Arg.Any<Func<IPortfolioItem, Guid>>(),
                Arg.Any<Func<IPortfolioItem, PortfolioItem>>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(items.ToLookup(x => x.Id));
        var dataLoader = new PortfolioItemByPortfolioCategoryIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioCategoryTypeExtension();

        var result = await teut.GetPortfolioItems(
            new PortfolioCategory { Id = Guid.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
