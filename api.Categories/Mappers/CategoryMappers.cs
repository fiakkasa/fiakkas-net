using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers;

public static class CategoryMappers
{
    private static T MapGenericCategory<T>(this ICategory x) 
    where T : class, IBaseData, ICategoryTitle, new()
    =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title
        };

    private static T MapGenericTechnologyCategory<T>(this ICategory x) 
    where T : class, ITechnologyCategory, new()
    =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            Href = x.Href
        };

    public static Category MapCategory(this ICategory x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Kind = x.Kind,
            Title = x.Title,
            Href = x.Href,
            AssociatedCategoryTypes = x.AssociatedCategoryTypes
        };

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
    
    public static TechnologyCategory MapTechnologyCategory(this ICategory x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Kind = x.Kind,
            Title = x.Title,
            Href = x.Href
        };

    public static OtherCategory MapOtherCategory(this ICategory x) =>
        x.MapGenericCategory<OtherCategory>();
}
