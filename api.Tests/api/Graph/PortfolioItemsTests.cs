using HotChocolate.Execution;

namespace api.Tests;

public class PortfolioItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task PortfolioItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  portfolioItems(order: { createdAt: ASC }) {
    totalCount
    items {
      categoryId
      customerId
      createdAt
      href
      id
      technologyIds
      technologiesSummary
      title
      updatedAt
      version
      year
      category {
        createdAt
        id
        title
        updatedAt
        version
      }
      customer {
        createdAt
        href
        id
        title
        updatedAt
        version
      }
      technologies(order: { createdAt: ASC }) {
        totalCount
        items {
          createdAt
          href
          id
          title
          updatedAt
          version
        }
      }
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
