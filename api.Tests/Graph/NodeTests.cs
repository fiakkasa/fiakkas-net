using HotChocolate.Execution;

namespace api.Tests.Graph;

public class NodeTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Nodes_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              nodes(
                ids: [
                  #Achievement
                  "QWNoaWV2ZW1lbnQKZ2Q0NjA1YjBjNThiYzQ5YWNiY2ZkMTBhMjRhMjAzYWRk"
                  #ContactItem
                  "Q29udGFjdEl0ZW0KZ2ViZjIyNGE4N2ZmMzQ3Yjk4ODJiZGQ0MWVjN2Y1YTA1"
                  #Customer
                  "Q3VzdG9tZXIKZzE4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5"
                  #EducationItem
                  "RWR1Y2F0aW9uSXRlbQpnMzg4OThjNjIxNjFlNDBmMjhhOWYzOWJmMWZmNDYyMjQ="
                  #InformationTechnologyCategory
                  "SW5mb3JtYXRpb25UZWNobm9sb2d5Q2F0ZWdvcnkKZzZjYzQzZTlhMzEyYjQ5MjNiODkwZTk2NmI4MTY4ZWVl"
                  #Language
                  "TGFuZ3VhZ2UKZzAyYTNiZTliM2YwNDRiNGE4OTQ1ZTg0ZmVmNTM3YjU4"
                  #OtherCategory
                  "T3RoZXJDYXRlZ29yeQpnOWZkOTFmOGEyYTQ0NDUyMGE1YjE4YWExYzNjMjkxMzM="
                  #PortfolioCategory
                  "UG9ydGZvbGlvQ2F0ZWdvcnkKZzM4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5"
                  #PortfolioItem
                  "UG9ydGZvbGlvSXRlbQpnMjhlNDgzZTQ2OTYxNGIyNTg4YTlkMWQwYTUxNjExMDk="
                  #ResumeCategory
                  "UmVzdW1lQ2F0ZWdvcnkKZ2ViOWQ2MjU4OTljNDQ2YmRiZDQ0MjNkMzViMTk5NjVk"
                  #SoftwareDevelopmentCategory
                  "U29mdHdhcmVEZXZlbG9wbWVudENhdGVnb3J5CmdjYTgzMmJmOWI3Y2I0YzMxYmY4ZDAwZjg3YTI3NmZlMw=="
                  #TextItem
                  "VGV4dEl0ZW0KZzQ4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5"
                  #UnknownCategory
                  "VW5rbm93bkNhdGVnb3J5CmdjOWY1ODc5ZDQwMTg0OWEwOWI3MWI0NzlkZDVkZTdmZg=="
                  #UnknownTechnologyCategory
                  "VW5rbm93blRlY2hub2xvZ3lDYXRlZ29yeQpnZTBlOWNhNWQ1MjYzNDRmNzhiMjNiM2NlMjAzMjMxZDQ="
                ]
              ) {
                __typename
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
                ... on InformationTechnologyCategory {
                  createdAt
                  href
                  id
                  internalId
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
                ... on UnknownCategory {
                  createdAt
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
            """);

        var fn = result.ExpectOperationResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
