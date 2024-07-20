using HotChocolate.Execution;

namespace api.Tests;

public class EducationItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task EducationItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  educationItems {
    totalCount
    nodes {
      categoryId
      createdAt
      description
      href
      id
      internalId
      location
      subjects
      title
      updatedAt
      version
      timePeriod {
        end
        start
      }
      category {
        associatedCategoryTypes
        createdAt
        id
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

    [Fact]
    public async Task EducationItemById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  educationItemById(id: "RWR1Y2F0aW9uSXRlbQpnMzg4OThjNjIxNjFlNDBmMjhhOWYzOWJmMWZmNDYyMjQ=") {
    categoryId
    createdAt
    description
    href
    id
    internalId
    location
    subjects
    title
    updatedAt
    version
    timePeriod {
      end
      start
    }
    category {
      associatedCategoryTypes
      createdAt
      id
      internalId
      title
      updatedAt
      version
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
