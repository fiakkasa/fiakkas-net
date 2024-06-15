using HotChocolate.Execution;

namespace api.Tests;

public class LanguageTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Languages_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  languages {
    totalCount
    items {
      createdAt
      id
      proficiency
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
