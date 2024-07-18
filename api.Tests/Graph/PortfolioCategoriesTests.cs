using HotChocolate.Execution;

namespace api.Tests;
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
    items {
      createdAt
      id
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
    }
}