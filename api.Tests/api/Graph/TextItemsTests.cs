using HotChocolate.Execution;

namespace api.Tests;

public class TextItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task TextItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  textItems {
    totalCount
    items {
      content
      createdAt
      id
      key
      title
      updatedAt
      version
    }
  }
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}

