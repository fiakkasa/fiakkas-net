using HotChocolate.Execution;

namespace api.Tests;

public class CategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Categories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  categories {
    totalCount
    nodes {
      associatedCategoryTypes
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
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task CategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  categoryById(id: "Q2F0ZWdvcnkKZ2M5ZjU4NzlkNDAxODQ5YTA5YjcxYjQ3OWRkNWRlN2Zm") {
    associatedCategoryTypes
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
