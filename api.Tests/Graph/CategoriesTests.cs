using HotChocolate.Execution;

namespace api.Tests;

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
    items {
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
      ... on OtherCategory {
        createdAt
        id
        title
        updatedAt
        version
      }
      ... on PortfolioCategory {
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
      ... on ResumeCategory {
        associatedCategoryTypes
        createdAt
        id
        title
        updatedAt
        version
        associatedCategories {
          totalCount
        }
        educationItems {
          totalCount
        }
      }
      ... on SoftwareDevelopmentCategory {
        createdAt
        href
        id
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
      ... on UnknownCategory {
        createdAt
        id
        title
        updatedAt
        version
      }
      ... on UnknownTechnologyCategory {
        createdAt
        href
        id
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