using api.Portfolio.Models;
using api.Portfolio.Services;

namespace api.Portfolio.Tests.Services;

public class PortfolioItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new PortfolioItemEntity
        {
            Id = new("28e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Year = 2024,
            CategoryId = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            Title = "Title",
            Href = new("/test", UriKind.Relative),
            TechnologyIds = [new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
            CustomerId = new("18e483e4-6961-4b25-88a9-d1d0a5161109")
        };
        var configData = new PortfolioDataConfig
        {
            PortfolioItems = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<PortfolioDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new PortfolioItemDataRepository(
            Substitute.For<ILogger<PortfolioItemDataRepository>>(),
            configOptions
        );

        var result = sut.Get();

        Assert.Single(result);
        result.MatchSnapshot();
    }
}
