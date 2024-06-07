using api.TextItems.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.TextItems.Services.Tests;

public class TextItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new TextItemEntity
        {
            Id = new Guid("2f69e973-550b-4769-801a-e757807e6845"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Title = "Title",
            Content = "Content"
        };
        var configData = new TextItemsDataConfig
        {
            TextItems = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<TextItemsDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new TextItemDataRepository(Substitute.For<ILogger<TextItemDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
