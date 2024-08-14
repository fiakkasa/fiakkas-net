using HotChocolate.Execution;

namespace api.Tests.Graph;

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
                  __typename
                  createdAt
                  id
                  title
                  updatedAt
                  version
                  ... on InformationTechnologyCategory {
                    createdAt
                    href
                    id
                    internalId
                    title
                    updatedAt
                    version
                  }
                  ... on OtherCategory {
                    createdAt
                    id
                    internalId
                    title
                    updatedAt
                    version
                  }
                  ... on PortfolioCategory {
                    createdAt
                    id
                    internalId
                    title
                    updatedAt
                    version
                  }
                  ... on ResumeCategory {
                    associatedCategoryTypes
                    createdAt
                    id
                    internalId
                    title
                    updatedAt
                    version
                  }
                  ... on SoftwareDevelopmentCategory {
                    createdAt
                    href
                    id
                    internalId
                    title
                    updatedAt
                    version
                  }
                  ... on UnknownCategory {
                    createdAt
                    id
                    internalId
                    title
                    updatedAt
                    version
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
