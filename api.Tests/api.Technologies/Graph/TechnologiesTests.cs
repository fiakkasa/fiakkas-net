using HotChocolate.Execution;

namespace api.Technologies.Tests;

public class TechnologiesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Technologies_Should_Return_Results()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  technologies(where: { version: { eq: 1 } }) {
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
    }
    pageInfo {
      hasNextPage
      hasPreviousPage
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
