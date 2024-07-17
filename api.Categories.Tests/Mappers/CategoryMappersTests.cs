using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers.Tests;

public class CategoryMappersTests
{
    public record CategoryMockEntity : ICategoryEntity
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
    public void MapGenericCategory_Should_Map_To_Type_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Portfolio,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapGenericCategory<PortfolioCategory>();

        result.Should().BeOfType<PortfolioCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapResumeCategory_Should_Map_To_ResumeCategory_And_Return_Data()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Resume,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
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
    public void MapTechnologyCategories_Should_Map_To_SoftwareDevelopmentCategory_And_Return_Data_When_Kind_Is_SoftwareDevelopment()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.SoftwareDevelopment,
            Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.MapTechnologyCategories();

        result.Should().BeOfType<SoftwareDevelopmentCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapTechnologyCategories_Should_Map_To_InformationTechnologyCategory_And_Return_Data_When_Kind_Is_InformationTechnology()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.MapTechnologyCategories();

        result.Should().BeOfType<InformationTechnologyCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void MapTechnologyCategories_Should_Map_To_UnknownTechnologyCategory_And_Return_Data_When_Kind_Is_Not_Resolved()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.None,
            Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.MapTechnologyCategories();

        result.Should().BeOfType<UnknownTechnologyCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void Map_Should_Map_To_PortfolioCategory_And_Return_Data_When_Kind_Is_Portfolio()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Portfolio,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.Map();

        result.Should().BeOfType<PortfolioCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void Map_Should_Map_To_ResumeCategory_And_Return_Data_When_Kind_Is_Resume()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Resume,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
        };

        var result = item.Map();

        result.Should().BeOfType<ResumeCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void Map_Should_Map_To_OtherCategory_And_Return_Data_When_Kind_Is_Other()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.Other,
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.Map();

        result.Should().BeOfType<OtherCategory>();
        result.MatchSnapshot();
    }

    [Fact]
    public void Map_Should_Map_To_SoftwareDevelopmentCategory_And_Return_Data_When_Kind_Is_SoftwareDevelopment()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.SoftwareDevelopment,
            Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.Map();

        result.Should().BeOfType<SoftwareDevelopmentCategory>();
        result.MatchSnapshot();

    }

    [Fact]
    public void Map_Should_Map_To_InformationTechnologyCategory_And_Return_Data_When_Kind_Is_InformationTechnology()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.InformationTechnology,
            Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.Map();

        result.Should().BeOfType<InformationTechnologyCategory>();
        result.MatchSnapshot();
    }


    [Fact]
    public void Map_Should_Map_To_UnknownCategory_And_Return_Data_When_Kind_Is_Not_Resolved()
    {
        var item = new CategoryMockEntity
        {
            Kind = CategoryType.None,
            Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };

        var result = item.Map();

        result.Should().BeOfType<UnknownCategory>();
        result.MatchSnapshot();
    }
}
