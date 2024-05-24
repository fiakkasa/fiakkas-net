using HotChocolate.Execution;

namespace api.Portfolio.Tests;

public class PortfolioCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task PortfolioCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  portfolioCategories(
    where: { version: { eq: 1 } }
    order: { createdAt: ASC }
  ) {
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioItems(
        where: { version: { eq: 1 } }
        order: { createdAt: ASC }
      ) {
        pageInfo {
          hasNextPage
          hasPreviousPage
        }
        items {
          createdAt
          href
          id
          ordinal
          title
          updatedAt
          version
          year
        }
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
