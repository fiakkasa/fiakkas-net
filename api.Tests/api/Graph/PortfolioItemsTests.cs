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
  portfolioItems {
    totalCount
    items {
      categoryId
      createdAt
      customerId
      href
      id
      technologiesSummary
      technologyIds
      title
      updatedAt
      version
      year
      customer {
        createdAt
        href
        id
        title
        updatedAt
        version
      }
      technologyCategories {
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
      category {
        createdAt
        id
        title
        updatedAt
        version
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
