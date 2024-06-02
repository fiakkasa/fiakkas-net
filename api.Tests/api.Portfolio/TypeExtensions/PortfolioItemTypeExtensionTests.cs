using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.TypeExtensions;

public class PortfolioItemTypeExtensionTests
{
    [Fact]
    public async Task GetCategory_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<IPortfolioCategory>(
        [
            new PortfolioCategory
            {
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);
        var dataLoader = new PortfolioCategoryBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new PortfolioItemTypeExtension();

        var result = await sut.GetCategory(
            new PortfolioItem { CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109") },
            dataLoader,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.MatchSnapshot();
    }
}
