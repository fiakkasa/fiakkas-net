using HotChocolate.Execution;

namespace api.Tests;

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
      ... on UnknownTechnologyCategory {
        createdAt
        href
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
