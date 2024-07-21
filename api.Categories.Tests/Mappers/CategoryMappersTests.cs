using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers.Tests;

public class CategoryMappersTests
{
    public record CategoryMockEntity : ICategory
    {
        public CategoryType Kind { get; init; }
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
        public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
    }

    [Fact]
    public void MapPolymorphicCategory_Should_Map_To_Type_And_Return_Data()
    {
        var items = new Dictionary<Type, CategoryMockEntity>
        {
            [typeof(UnknownCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            [typeof(PortfolioCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.Portfolio,
                Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            [typeof(ResumeCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.Resume,
                Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
            },
            [typeof(SoftwareDevelopmentCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            },
            [typeof(InformationTechnologyCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.InformationTechnology,
                Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            },
            [typeof(OtherCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.Other,
                Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        };

        var result = 
            items.Values
                .Select(x => x.MapPolymorphicCategory())
                .ToArray();

        result.Should().HaveCount(items.Count);
        result.Select(x => x.GetType()).Should().BeEquivalentTo(items.Keys);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapPolymorphicTechnologyCategory_Should_Map_To_Type_And_Return_Data()
    {
        var items = new Dictionary<Type, CategoryMockEntity>
        {
            [typeof(UnknownTechnologyCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.None,
                Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            },
            [typeof(SoftwareDevelopmentCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            },
            [typeof(InformationTechnologyCategory)] = new CategoryMockEntity
            {
                Kind = CategoryType.InformationTechnology,
                Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        };

        var result = 
            items.Values
                .Select(x => x.MapPolymorphicTechnologyCategory())
                .ToArray();

        result.Should().HaveCount(items.Count);
        result.Select(x => x.GetType()).Should().BeEquivalentTo(items.Keys);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapUnknownCategory_Should_Map_To_Category_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.None,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };

        var result = item.MapUnknownCategory();

        result.Should().BeOfType<UnknownCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapPortfolioCategory_Should_Map_To_PortfolioCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Portfolio,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapPortfolioCategory();

        result.Should().BeOfType<PortfolioCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapResumeCategory_Should_Map_To_ResumeCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Resume,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };

        var result = item.MapResumeCategory();

        result.Should().BeOfType<ResumeCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapSoftwareDevelopmentCategory_Should_Map_To_SoftwareDevelopmentCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.SoftwareDevelopment,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.MapSoftwareDevelopmentCategory();

        result.Should().BeOfType<SoftwareDevelopmentCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapInformationTechnologyCategory_Should_Map_To_InformationTechnologyCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.MapInformationTechnologyCategory();

        result.Should().BeOfType<InformationTechnologyCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapOtherCategory_Should_Map_To_OtherCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Other,
            Id = new Guid("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapOtherCategory();

        result.Should().BeOfType<OtherCategory>();
        result.MatchSnapshot();
    }
}
