using HotChocolate.Execution;

namespace api.Tests.Graph;

public partial class SoftwareDevelopmentCategoriesTests
{
    public class PortfolioCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
    {
        [Fact]
        public async Task PortfolioCategories_Should_Return_Data()
        {
            var executor = await fixture.GetRequestExecutor();

            var result = await executor.ExecuteAsync(
                """
                {
                  portfolioCategories {
                    totalCount
                    nodes {
                      createdAt
                      id
                      internalId
                      title
                      updatedAt
                      version
                      customers {
                        totalCount
                      }
                      portfolioItems {
                        totalCount
                      }
                      technologyCategories {
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
        public async Task PortfolioCategoryById_Should_Return_Data()
        {
            var executor = await fixture.GetRequestExecutor();

            var result = await executor.ExecuteAsync(
                """
                {
                  portfolioCategoryById(id: "UG9ydGZvbGlvQ2F0ZWdvcnkKZzM4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5") {
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
}
