using HotChocolate.Execution;

namespace api.Customers.Tests;

public class CustomersTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Customers_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  customers(
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
