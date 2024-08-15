using HotChocolate.Execution;

namespace api.Tests.Graph;

public class CustomersTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Customers_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              customers {
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
                  portfolioItems {
                    totalCount
                  }
                  portfolioTechnologyCategories {
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
    public async Task CustomerById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              customerById(id: "Q3VzdG9tZXIKZzE4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5") {
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

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
