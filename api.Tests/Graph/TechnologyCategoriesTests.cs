using HotChocolate.Execution;

namespace api.Tests.Graph;

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
                  __typename
                  createdAt
                  href
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
                  ... on SoftwareDevelopmentCategory {
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
                  ... on UnknownTechnologyCategory {
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
            }
            """);

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
