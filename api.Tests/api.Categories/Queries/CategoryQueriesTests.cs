using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Queries.Tests;

public class CategoryQueriesTests
{
    [Fact]
    public void GetCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.Portfolio,
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.Resume,
                Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var sut = new CategoryQueries();

        var result = sut.GetCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<ICategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetPortfolioCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.Portfolio,
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);
        var sut = new CategoryQueries();

        var result = sut.GetPortfolioCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<PortfolioCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetResumeCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.Resume,
                Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);
        var sut = new CategoryQueries();

        var result = sut.GetResumeCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<ResumeCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetSoftwareDevelopmentCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var sut = new CategoryQueries();

        var result = sut.GetSoftwareDevelopmentCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<ICategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public void GetTechnologyCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategoryEntity>(
        [
            new CategoryEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            new CategoryEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);
        var sut = new CategoryQueries();

        var result = sut.GetTechnologyCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<ITechnologyCategory>>();
        result.MatchSnapshot();
    }
}
