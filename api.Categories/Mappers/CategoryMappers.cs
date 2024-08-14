using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers;

public static class CategoryMappers
{
    private static T MapGenericCategory<T>(this ICategory x)
        where T : class, IBaseData, ICategoryTitle, new()
        => new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title
        };

    private static T MapGenericTechnologyCategory<T>(this ICategory x)
        where T : class, ITechnologyCategory, new()
        => new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            Href = x.Href
        };

    public static IPolymorphicCategory MapPolymorphicCategory(this ICategory x) =>
        x switch
        {
            { Kind: CategoryType.Portfolio } => x.MapPortfolioCategory(),
            { Kind: CategoryType.Resume } => x.MapResumeCategory(),
            { Kind: CategoryType.Other } => x.MapOtherCategory(),
            { Kind: CategoryType.SoftwareDevelopment } => x.MapSoftwareDevelopmentCategory(),
            { Kind: CategoryType.InformationTechnology } => x.MapInformationTechnologyCategory(),
            _ => x.MapUnknownCategory()
        };

    public static IPolymorphicTechnologyCategory MapPolymorphicTechnologyCategory(this ICategory x) =>
        x switch
        {
            { Kind: CategoryType.SoftwareDevelopment } => x.MapSoftwareDevelopmentCategory(),
            { Kind: CategoryType.InformationTechnology } => x.MapInformationTechnologyCategory(),
            _ => x.MapGenericTechnologyCategory<UnknownTechnologyCategory>()
        };

    public static UnknownCategory MapUnknownCategory(this ICategory x) => x.MapGenericCategory<UnknownCategory>();

    public static PortfolioCategory MapPortfolioCategory(this ICategory x) =>
        x.MapGenericCategory<PortfolioCategory>();

    public static ResumeCategory MapResumeCategory(this ICategory x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            AssociatedCategoryTypes = x.AssociatedCategoryTypes
        };

    public static SoftwareDevelopmentCategory MapSoftwareDevelopmentCategory(this ICategory x) =>
        x.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>();

    public static InformationTechnologyCategory MapInformationTechnologyCategory(this ICategory x) =>
        x.MapGenericTechnologyCategory<InformationTechnologyCategory>();

    public static OtherCategory MapOtherCategory(this ICategory x) =>
        x.MapGenericCategory<OtherCategory>();
}
