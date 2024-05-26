using api.Portfolio.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Portfolio.Services;

public class PortfolioCategoryDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new PortfolioCategoryEntity
        {
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var configData = new PortfolioDataConfig
        {
            PortfolioCategories = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<PortfolioDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new PortfolioCategoryDataRepository(Substitute.For<ILogger<PortfolioCategoryDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
