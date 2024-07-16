using HotChocolate.Execution;

namespace api.Tests;

public class InformationTechnologyCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task InformationTechnologyCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  informationTechnologyCategories {
    totalCount
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioCategories {
        totalCount
      }
      portfolioCustomers {
        totalCount
      }
      portfolioItems {
        totalCount
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
