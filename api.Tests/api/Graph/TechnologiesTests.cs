using HotChocolate.Execution;

namespace api.Tests;

public class TechnologiesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Technologies_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
query {
  technologies(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
    totalCount
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      customers(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
        totalCount
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
      portfolioCategories(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
        totalCount
        items {
          createdAt
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
      portfolioItems(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
        totalCount
        pageInfo {
          hasNextPage
          hasPreviousPage
        }
        items {
          createdAt
          href
          id
          ordinal
          technologiesSummary
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
