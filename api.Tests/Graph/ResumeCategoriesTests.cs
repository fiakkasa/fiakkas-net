using HotChocolate.Execution;

namespace api.Tests.Graph;

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
                nodes {
                  associatedCategoryTypes
                  createdAt
                  id
                  internalId
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

        var fn = result.ExpectOperationResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task ResumeCategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              resumeCategoryById(id: "UmVzdW1lQ2F0ZWdvcnkKZ2ViOWQ2MjU4OTljNDQ2YmRiZDQ0MjNkMzViMTk5NjVk") {
                associatedCategoryTypes
                createdAt
                id
                internalId
                title
                updatedAt
                version
              }
            }
            """);

        var fn = result.ExpectOperationResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
