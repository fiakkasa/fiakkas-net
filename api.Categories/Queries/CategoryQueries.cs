using api.Categories.DataLoaders;
using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.Queries;

[QueryType]
public static class CategoryQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<ICategory> GetCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(CategoryMappers.Map);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<PortfolioCategory> GetPortfolioCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsPortfolioCategory,
            CategoryMappers.MapGenericCategory<PortfolioCategory>
        );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<ResumeCategory> GetResumeCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsResumeCategory,
            CategoryMappers.MapResumeCategory
        );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<SoftwareDevelopmentCategory> GetSoftwareDevelopmentCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsSoftwareDevelopmentCategory,
            CategoryMappers.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>
        );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<InformationTechnologyCategory> GetInformationTechnologyCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsInformationTechnologyCategory,
            CategoryMappers.MapGenericTechnologyCategory<InformationTechnologyCategory>
        );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<ITechnologyCategory> GetTechnologyCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsTechnologyCategory,
            CategoryMappers.MapTechnologyCategories
        );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<OtherCategory> GetOtherCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsOtherCategory,
            CategoryMappers.MapGenericCategory<OtherCategory>
        );
}
