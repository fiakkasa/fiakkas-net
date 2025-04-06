using HotChocolate.Execution;

namespace api.Tests.Graph;

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

        Assert.Null(fn().Errors);
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task InformationTechnologyCategoryById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              informationTechnologyCategoryById(id: "SW5mb3JtYXRpb25UZWNobm9sb2d5Q2F0ZWdvcnkKZzZjYzQzZTlhMzEyYjQ5MjNiODkwZTk2NmI4MTY4ZWVl") {
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

        Assert.Null(fn().Errors);
        result.ToJson().MatchSnapshot();
    }
}
