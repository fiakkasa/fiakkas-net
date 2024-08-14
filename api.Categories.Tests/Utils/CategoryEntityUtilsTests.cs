using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Utils;

namespace api.Categories.Tests.Utils;

public class CategoryEntityUtilsTests
{
    [Theory]
    [InlineData(CategoryType.None, true)]
    [InlineData(CategoryType.SoftwareDevelopment, false)]
    public void IsUnknownCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsUnknownCategory(item);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(CategoryType.Portfolio, true)]
    [InlineData(CategoryType.None, false)]
    public void IsPortfolioCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsPortfolioCategory(item);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(CategoryType.Resume, true)]
    [InlineData(CategoryType.None, false)]
    public void IsResumeCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsResumeCategory(item);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(CategoryType.SoftwareDevelopment, true)]
    [InlineData(CategoryType.None, false)]
    public void IsSoftwareDevelopmentCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsSoftwareDevelopmentCategory(item);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(CategoryType.SoftwareDevelopment, true)]
    [InlineData(CategoryType.InformationTechnology, true)]
    [InlineData(CategoryType.None, false)]
    public void IsTechnologyCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsTechnologyCategory(item);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(CategoryType.Other, true)]
    [InlineData(CategoryType.None, false)]
    public void IsOtherCategory_Should_Return_True_When_Matched(CategoryType kind, bool expected)
    {
        var item = new CategoryMockEntity
        {
            Kind = kind
        };

        var result = CategoryEntityUtils.IsOtherCategory(item);

        result.Should().Be(expected);
    }

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
}
