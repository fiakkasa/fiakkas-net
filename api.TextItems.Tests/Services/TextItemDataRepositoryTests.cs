using api.TextItems.Models;
using api.TextItems.Services;

namespace api.TextItems.Tests.Services;

public class TextItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new TextItemEntity
        {
            Id = new("2f69e973-550b-4769-801a-e757807e6845"),
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

        Assert.Single(result);
        result.MatchSnapshot();
    }
}
