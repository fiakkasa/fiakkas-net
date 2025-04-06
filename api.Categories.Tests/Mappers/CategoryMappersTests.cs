using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;

namespace api.Categories.Tests.Mappers;

public class CategoryMappersTests
{
    [Fact]
    public void MapPolymorphicCategory_Should_Map_To_Type_And_Return_Data()
    {
        var items = new Dictionary<Type, CategoryMockEntity>
        {
            [typeof(UnknownCategory)] =
                new()
                {
                    Kind = CategoryType.None,
                    Id = new("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                },
            [typeof(PortfolioCategory)] =
                new()
                {
                    Kind = CategoryType.Portfolio,
                    Id = new("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                },
            [typeof(ResumeCategory)] =
                new()
                {
                    Kind = CategoryType.Resume,
                    Id = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
                },
            [typeof(SoftwareDevelopmentCategory)] =
                new()
                {
                    Kind = CategoryType.SoftwareDevelopment,
                    Id = new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new("/test", UriKind.Relative)
                },
            [typeof(InformationTechnologyCategory)] = new()
            {
                Kind = CategoryType.InformationTechnology,
                Id = new("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            },
            [typeof(OtherCategory)] = new()
            {
                Kind = CategoryType.Other,
                Id = new("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title"
            }
        };

        var result =
            items
                .Values
                .Select(x => x.MapPolymorphicCategory())
                .ToArray();
        var resultTypes =
            result
                .Select(x => x.GetType().ToString())
                .Order()
                .ToArray();

        Assert.Equal(items.Count, result.Length);
        Assert.All(
            items.Keys.Select(x => x.ToString()).Order().ToArray(),
            (x, i) => Assert.Equal(x, resultTypes[i])
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void MapPolymorphicTechnologyCategory_Should_Map_To_Type_And_Return_Data()
    {
        var items = new Dictionary<Type, CategoryMockEntity>
        {
            [typeof(UnknownTechnologyCategory)] =
                new()
                {
                    Kind = CategoryType.None,
                    Id = new("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                },
            [typeof(SoftwareDevelopmentCategory)] = new()
            {
                Kind = CategoryType.SoftwareDevelopment,
                Id = new("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            },
            [typeof(InformationTechnologyCategory)] = new()
            {
                Kind = CategoryType.InformationTechnology,
                Id = new("6cc43e9a-312b-4923-b890-e966b8168eee"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new("/test", UriKind.Relative)
            }
        };
        var expectedKeys =
            items.Keys
                .Select(x => x.ToString())
                .Order()
                .ToArray();

        var result =
            items
                .Values
                .Select(x => x.MapPolymorphicTechnologyCategory())
                .ToArray();
        var resultKeys =
            result
                .Select(x => x.GetType().ToString())
                .Order()
                .ToArray();

        Assert.Equal(items.Count, result.Length);
        Assert.All(
            expectedKeys,
            (x, i) => Assert.Equal(x, resultKeys[i])
        );
        result.MatchSnapshot();
    }

    [Fact]
    public void MapUnknownCategory_Should_Map_To_Category_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.None,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new("/test", UriKind.Relative),
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };

        var result = item.MapUnknownCategory();

        Assert.IsType<UnknownCategory>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapPortfolioCategory_Should_Map_To_PortfolioCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Portfolio,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapPortfolioCategory();

        Assert.IsType<PortfolioCategory>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapResumeCategory_Should_Map_To_ResumeCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Resume,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };

        var result = item.MapResumeCategory();

        Assert.IsType<ResumeCategory>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapSoftwareDevelopmentCategory_Should_Map_To_SoftwareDevelopmentCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.SoftwareDevelopment,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new("/test", UriKind.Relative)
        };

        var result = item.MapSoftwareDevelopmentCategory();

        Assert.IsType<SoftwareDevelopmentCategory>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapInformationTechnologyCategory_Should_Map_To_InformationTechnologyCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new("/test", UriKind.Relative)
        };

        var result = item.MapInformationTechnologyCategory();

        Assert.IsType<InformationTechnologyCategory>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public void MapOtherCategory_Should_Map_To_OtherCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Other,
            Id = new("e18eff98-1d52-4b33-b232-60f8e98a3603"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapOtherCategory();

        Assert.IsType<OtherCategory>(result);
        result.MatchSnapshot();
    }

    private record CategoryMockEntity : ICategory
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
}
