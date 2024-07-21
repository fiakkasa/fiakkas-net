using HotChocolate.Execution;

namespace api.Tests;

public class TechnologyCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task TechnologyCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  technologyCategories {
    totalCount
    nodes {
      createdAt
      href
      id
      internalId
      kind
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

    [Fact]
    public async Task TechnologyCategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  technologyCategoryById(id: "VGVjaG5vbG9neUNhdGVnb3J5CmdjYTgzMmJmOWI3Y2I0YzMxYmY4ZDAwZjg3YTI3NmZlMw==") {
    createdAt
    href
    id
    internalId
    kind
    title
    updatedAt
    version
  }
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
