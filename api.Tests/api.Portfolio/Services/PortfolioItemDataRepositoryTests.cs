using api.Portfolio.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Portfolio.Services.Tests;

public class PortfolioItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new PortfolioItemEntity
        {
            Id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109")],
            CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };
        var configData = new PortfolioDataConfig
        {
            PortfolioItems = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<PortfolioDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new PortfolioItemDataRepository(Substitute.For<ILogger<PortfolioItemDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
