using api.Portfolio.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Portfolio.Services;

public class PortfolioItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new PortfolioItemEntity
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = Guid.Empty,
            Ordinal = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            TechnologyIds = [Guid.Empty],
            CustomerId = Guid.Empty
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
