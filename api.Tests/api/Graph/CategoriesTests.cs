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
        items {
          createdAt
          href
          id
          title
          updatedAt
          version
        }
      }
      portfolioItems {
        totalCount
        items {
          categoryId
          createdAt
          customerId
          href
          id
          technologiesSummary
          technologyIds
          title
          updatedAt
          version
          year
        }
      }
      technologyCategories {
        totalCount
        items {
          createdAt
          href
          id
          title
          updatedAt
          version
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

    [Fact]
    public async Task ResumeCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  resumeCategories {
    totalCount
    items {
      associatedCategoryTypes
      createdAt
      id
      title
      updatedAt
      version
      associatedCategories {
        totalCount
        items {
          __typename
          createdAt
          id
          title
          updatedAt
          version
          ... on Category {
            createdAt
            id
            title
            updatedAt
            version
          }
          ... on InformationTechnologyCategory {
            createdAt
            href
            id
            title
            updatedAt
            version
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
          }
          ... on ResumeCategory {
            associatedCategoryTypes
            createdAt
            id
            title
            updatedAt
            version
          }
          ... on SoftwareDevelopmentCategory {
            createdAt
            href
            id
            title
            updatedAt
            version
          }
          ... on TechnologyCategory {
            createdAt
            href
            id
            title
            updatedAt
            version
          }
        }
      }
      educationItems {
        totalCount
        items {
          categoryId
          createdAt
          description
          href
          id
          location
          subjects
          title
          updatedAt
          version
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

    [Fact]
    public async Task InformationTechnologyCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  informationTechnologyCategories {
    totalCount
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioCategories {
        totalCount
        items {
          createdAt
          id
          title
          updatedAt
          version
        }
      }
      portfolioCustomers {
        totalCount
        items {
          createdAt
          href
          id
          title
          updatedAt
          version
        }
      }
      portfolioItems {
        totalCount
        items {
          categoryId
          createdAt
          customerId
          href
          id
          technologiesSummary
          technologyIds
          title
          updatedAt
          version
          year
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

    [Fact]
    public async Task SoftwareDevelopmentCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  softwareDevelopmentCategories {
    totalCount
    items {
      createdAt
      href
      id
      title
      updatedAt
      version
      portfolioCategories {
        totalCount
        items {
          createdAt
          id
          title
          updatedAt
          version
        }
      }
      portfolioCustomers {
        totalCount
        items {
          createdAt
          href
          id
          title
          updatedAt
          version
        }
      }
      portfolioItems {
        totalCount
        items {
          categoryId
          createdAt
          customerId
          href
          id
          technologiesSummary
          technologyIds
          title
          updatedAt
          version
          year
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

    [Fact]
    public async Task TechnologyCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  technologyCategories {
    totalCount
    items {
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
        title
        updatedAt
        version
      }
      ... on SoftwareDevelopmentCategory {
        createdAt
        href
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
    public async Task OtherCategories_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
  otherCategories {
    totalCount
    items {
      createdAt
      id
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
