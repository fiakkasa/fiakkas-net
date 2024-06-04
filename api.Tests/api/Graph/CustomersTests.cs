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
  customers(order: { createdAt: ASC }) {
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
      portfolioItems(order: { createdAt: ASC }) {
        totalCount
        items {
          categoryId
          createdAt
          customerId
          href
          id
          ordinal
          technologyIds
          technologiesSummary
          title
          updatedAt
          version
          year
        }
      }
      portfolioTechnologies(order: { createdAt: ASC }) {
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
