using api.Technologies.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Technologies.Services.Tests;

public class TechnologyDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new TechnologyEntity
        {
            Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var configData = new TechnologiesDataConfig
        {
            Technologies = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<TechnologiesDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new TechnologyDataRepository(Substitute.For<ILogger<TechnologyDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
