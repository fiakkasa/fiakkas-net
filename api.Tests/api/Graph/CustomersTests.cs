using HotChocolate.Execution;

namespace api.Tests;

public class CustomersTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Customers_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  customers(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
    totalCount
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioCategories(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
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
      portfolioItems(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
        totalCount
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
        pageInfo {
          hasNextPage
          hasPreviousPage
        }
      }
      technologies(order: { createdAt: ASC }, take: 1, where: { version: { eq: 1 } }) {
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
