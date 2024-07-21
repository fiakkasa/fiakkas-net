using api.TextItems.DataLoaders;
using api.TextItems.Interfaces;
using api.TextItems.Models;
using GreenDonut;

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

        var result = TextItemQueries.GetTextItems(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<TextItem>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTextItemById_Should_Return_Data_When_Found()
    {
        var id = new Guid("2f69e973-550b-4769-801a-e757807e6845");
        var item = new TextItem
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Key = "Key",
            Title = "Title",
            Content = "Content"
        };
        var dataRepository = new MockDataRepository<ITextItem>([item]);
        var dataLoader = new TextItemBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await TextItemQueries.GetTextItemById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeOfType<TextItem>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTextItemById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("2f69e973-550b-4769-801a-e757807e6845");
        var dataRepository = new MockDataRepository<ITextItem>([]);
        var dataLoader = new TextItemBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await TextItemQueries.GetTextItemById(id, dataLoader, default);

        result.Should().BeNull();
    }
}
