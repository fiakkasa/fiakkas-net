using HotChocolate.Execution;

namespace api.Tests;

public class ResumeCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task ResumeCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  resumeCategories {
    totalCount
    items {
      associatedCategoryTypes
      createdAt
      id
      title
      updatedAt
      version
      associatedCategories {
        totalCount
      }
      educationItems {
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
