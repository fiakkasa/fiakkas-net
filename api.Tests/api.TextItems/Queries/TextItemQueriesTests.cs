using api.TextItems.Interfaces;
using api.TextItems.Models;

namespace api.TextItems.Queries.Tests;

public class TextItemQueriesTests
{
    [Fact]
    public void GetTextItems_Should_Return_Data()
    {
        var item = new TextItem
        {
            Id = new Guid("2f69e973-550b-4769-801a-e757807e6845"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Title = "Title",
            Content = "Content"
        };
        var dataRepository = new MockDataRepository<ITextItem>([item]);
        var sut = new TextItemQueries();

        var result = sut.GetTextItems(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<TextItem>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
