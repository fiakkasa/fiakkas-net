using HotChocolate.Execution;

namespace api.Tests;

public class AchievementsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Achievements_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  achievements {
    totalCount
    items {
      content
      createdAt
      id
      updatedAt
      version
      years
      yearsSummary
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
