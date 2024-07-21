using HotChocolate.Execution;

namespace api.Tests;

public class UnknownCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task UnknownCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  unknownCategories {
    totalCount
    nodes {
      createdAt
      id
      internalId
      title
      updatedAt
      version
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
    public async Task UnknownCategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  unknownCategoryById(id: "VW5rbm93bkNhdGVnb3J5CmdjOWY1ODc5ZDQwMTg0OWEwOWI3MWI0NzlkZDVkZTdmZg==") {
    createdAt
    id
    internalId
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

