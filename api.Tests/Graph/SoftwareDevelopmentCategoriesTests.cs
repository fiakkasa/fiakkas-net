using HotChocolate.Execution;

namespace api.Tests.Graph;

public partial class SoftwareDevelopmentCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task SoftwareDevelopmentCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              softwareDevelopmentCategories {
                totalCount
                nodes {
                  createdAt
                  href
                  id
                  internalId
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

        var fn = result.ExpectOperationResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task SoftwareDevelopmentCategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              softwareDevelopmentCategoryById(id: "U29mdHdhcmVEZXZlbG9wbWVudENhdGVnb3J5CmdjYTgzMmJmOWI3Y2I0YzMxYmY4ZDAwZjg3YTI3NmZlMw==") {
                createdAt
                href
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
