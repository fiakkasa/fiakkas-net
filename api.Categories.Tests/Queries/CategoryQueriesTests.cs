using api.Categories.DataLoaders;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Queries.Tests;

public class CategoryQueriesTests
{
    [Fact]
    public void GetCategories_Should_Return_Data()
    {
        var collection = new[]
        {
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
                Title = "Title",
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
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
            },
            new CategoryEntity
            {
                Kind = CategoryType.InformationTechnology,
                Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            },
            new CategoryEntity
            {
                Kind = CategoryType.Other,
                Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        };
        var dataRepository = new MockDataRepository<ICategory>(collection);

        var result = CategoryQueries.GetCategories(dataRepository);

        result.Should().HaveCount(collection.Length);
        result.Should().BeAssignableTo<IQueryable<ICategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133");
        var item = new CategoryEntity
        {
            Kind = CategoryType.Other,
            Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new CategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Category>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new CategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetPortfolioCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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

        var result = CategoryQueries.GetPortfolioCategories(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<PortfolioCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109");
        var item = new CategoryEntity
        {
            Kind = CategoryType.Portfolio,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new PortfolioCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetPortfolioCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PortfolioCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetPortfolioCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new PortfolioCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetPortfolioCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetResumeCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
                Title = "Title",
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
            }
        ]);

        var result = CategoryQueries.GetResumeCategories(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<ResumeCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetResumeCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d");
        var item = new CategoryEntity
        {
            Kind = CategoryType.Resume,
            Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new ResumeCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetResumeCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ResumeCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetResumeCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new ResumeCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetResumeCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetSoftwareDevelopmentCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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

        var result = CategoryQueries.GetSoftwareDevelopmentCategories(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<SoftwareDevelopmentCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetSoftwareDevelopmentCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3");
        var item = new CategoryEntity
        {
            Kind = CategoryType.SoftwareDevelopment,
            Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new SoftwareDevelopmentCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetSoftwareDevelopmentCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<SoftwareDevelopmentCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetSoftwareDevelopmentCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new SoftwareDevelopmentCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetSoftwareDevelopmentCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetInformationTechnologyCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
                Kind = CategoryType.InformationTechnology,
                Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);

        var result = CategoryQueries.GetInformationTechnologyCategories(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<InformationTechnologyCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetInformationTechnologyCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee");
        var item = new CategoryEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new InformationTechnologyCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetInformationTechnologyCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<InformationTechnologyCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetInformationTechnologyCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new InformationTechnologyCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetInformationTechnologyCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetTechnologyCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
            },
            new CategoryEntity
            {
                Kind = CategoryType.InformationTechnology,
                Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);

        var result = CategoryQueries.GetTechnologyCategories(dataRepository);

        result.Should().HaveCount(2);
        result.Should().BeAssignableTo<IQueryable<ITechnologyCategory>>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologyCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee");
        var item = new CategoryEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new TechnologyCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetTechnologyCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<TechnologyCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetTechnologyCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new TechnologyCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetTechnologyCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }

    [Fact]
    public void GetOtherCategories_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<ICategory>(
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
                Kind = CategoryType.Other,
                Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        ]);

        var result = CategoryQueries.GetOtherCategories(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<OtherCategory>>();
        result.MatchSnapshot();
    }

     [Fact]
    public async Task GetOtherCategoryById_Should_Return_Data_When_Found()
    {
        var id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133");
        var item = new CategoryEntity
        {
            Kind = CategoryType.Other,
            Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var dataLoader = new OtherCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetOtherCategoryById(id, dataLoader, default);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OtherCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetOtherCategoryById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<ICategory>([]);
        var dataLoader = new OtherCategoryBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await CategoryQueries.GetOtherCategoryById(id, dataLoader, default);

        result.Should().BeNull();
    }
}
