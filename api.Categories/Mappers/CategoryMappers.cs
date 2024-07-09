using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Mappers;

public static class CategoryMappers
{
    public static T MapGenericCategory<T>(this ICategoryEntity x) where T : class, ICategory, new() =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title
        };

    public static ResumeCategory MapResumeCategory(this ICategoryEntity x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            AssociatedCategoryTypes = x.AssociatedCategoryTypes
        };

    public static T MapGenericTechnologyCategory<T>(this ICategoryEntity x) where T : class, ITechnologyCategory, new() =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            Href = x.Href
        };

    public static ITechnologyCategory MapTechnologyCategories(this ICategoryEntity x) =>
        x switch
        {

            { Kind: CategoryType.SoftwareDevelopment } => x.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>(),
            { Kind: CategoryType.InformationTechnology } => x.MapGenericTechnologyCategory<InformationTechnologyCategory>(),
            _ => x.MapGenericTechnologyCategory<TechnologyCategory>()
        };

    public static ICategory Map(this ICategoryEntity x) =>
        x switch
        {
            { Kind: CategoryType.Portfolio } => x.MapGenericCategory<PortfolioCategory>(),
            { Kind: CategoryType.Resume } => x.MapResumeCategory(),
            { Kind: CategoryType.Other } => x.MapGenericCategory<OtherCategory>(),
            { Kind: CategoryType.SoftwareDevelopment } => x.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>(),
            { Kind: CategoryType.InformationTechnology } => x.MapGenericTechnologyCategory<InformationTechnologyCategory>(),
            _ => x.MapGenericCategory<Category>()
        };
}
