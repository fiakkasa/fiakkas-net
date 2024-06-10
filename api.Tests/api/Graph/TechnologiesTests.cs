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
{
  technologies(order: { createdAt: ASC }) {
    totalCount
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioCategories(order: { createdAt: ASC }) {
        totalCount
        items {
          createdAt
          id
          title
          updatedAt
          version
        }
      }
      portfolioCustomers(order: { createdAt: ASC }) {
        totalCount
        items {
          createdAt
          id
          title
          updatedAt
          version
        }
      }
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
