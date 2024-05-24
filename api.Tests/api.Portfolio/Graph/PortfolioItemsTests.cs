using HotChocolate.Execution;

namespace api.Portfolio.Tests;

public class PortfolioItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task PortfolioItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  portfolioItems(
    where: { version: { eq: 1 } }
    order: { createdAt: ASC }
  ) {
    items {
      createdAt
      href
      id
      ordinal
      title
      updatedAt
      version
      year
      category {
        createdAt
        href
        id
        title
        updatedAt
        version
      }
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
