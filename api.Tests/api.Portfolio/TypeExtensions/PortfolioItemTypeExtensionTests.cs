using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;

namespace api.Portfolio.TypeExtensions;

public class PortfolioItemTypeExtensionTests
{
    [Fact]
    public async Task GetCategory_Should_Return_Data()
    {
        var items = new[]
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
        };
        var dataRepository = Substitute.For<IDataRepository<IPortfolioCategory>>();
        dataRepository
            .GetBatch(
                Arg.Any<IReadOnlyList<Guid>>(),
                Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(items.ToDictionary(x => x.Id));

        var dataLoader = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var teut = new PortfolioItemTypeExtension();

        var result = await teut.GetCategory(
            new PortfolioItem { CategoryId = Guid.Empty, Title = string.Empty },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }
}
