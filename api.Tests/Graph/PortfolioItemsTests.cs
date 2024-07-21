using HotChocolate.Execution;

namespace api.Tests;

public class PortfolioItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task PortfolioItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  portfolioItems {
    totalCount
    nodes {
      categoryId
      createdAt
      customerId
      href
      id
      internalId
      technologiesSummary
      technologyIds
      title
      updatedAt
      version
      year
      category {
        createdAt
        id
        title
        updatedAt
        version
      }
      customer {
        createdAt
        href
        id
        title
        updatedAt
        version
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
    public async Task PortfolioItemById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  portfolioItemById(id: "UG9ydGZvbGlvSXRlbQpnMjhlNDgzZTQ2OTYxNGIyNTg4YTlkMWQwYTUxNjExMDk=") {
    categoryId
    createdAt
    customerId
    href
    id
    internalId
    technologiesSummary
    technologyIds
    title
    updatedAt
    version
    year
  }
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
