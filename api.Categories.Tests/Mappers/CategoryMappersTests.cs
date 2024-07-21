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
    public void MapCategory_Should_Map_To_Category_And_Return_Data()
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

        var result = item.MapCategory();

        result.Should().BeOfType<Category>();
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
    public void MapTechnologyCategory_Should_Map_To_InformationTechnologyCategory_And_Return_Data()
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

        var result = item.MapTechnologyCategory();

        result.Should().BeOfType<TechnologyCategory>();
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
