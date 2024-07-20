using HotChocolate.Execution;

namespace api.Tests;

public class NodeTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Nodes_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  nodes(ids: [
    "QWNoaWV2ZW1lbnQKZ2Q0NjA1YjBjNThiYzQ5YWNiY2ZkMTBhMjRhMjAzYWRk"
    "Q29udGFjdEl0ZW0KZ2ViZjIyNGE4N2ZmMzQ3Yjk4ODJiZGQ0MWVjN2Y1YTA1",
    "Q3VzdG9tZXIKZzE4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5",
    "RWR1Y2F0aW9uSXRlbQpnMzg4OThjNjIxNjFlNDBmMjhhOWYzOWJmMWZmNDYyMjQ=",
    "TGFuZ3VhZ2UKZzAyYTNiZTliM2YwNDRiNGE4OTQ1ZTg0ZmVmNTM3YjU4",
    "UG9ydGZvbGlvSXRlbQpnMjhlNDgzZTQ2OTYxNGIyNTg4YTlkMWQwYTUxNjExMDk=",
    "VGV4dEl0ZW0KZzQ4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5"
  ]) {
    id
    ... on Achievement {
      content
      createdAt
      id
      internalId
      updatedAt
      version
      years
      yearsSummary
    }
    ... on ContactItem {
      createdAt
      description
      href
      icon
      id
      internalId
      key
      title
      updatedAt
      version
    }
    ... on Customer {
      createdAt
      href
      id
      internalId
      title
      updatedAt
      version
    }
    ... on EducationItem {
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
    }
    ... on Language {
      createdAt
      id
      internalId
      proficiency
      title
      updatedAt
      version
    }
    ... on PortfolioItem {
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
    ... on TextItem {
      content
      createdAt
      id
      internalId
      key
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
