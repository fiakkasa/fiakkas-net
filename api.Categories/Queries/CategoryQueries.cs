using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;

namespace api.Categories.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class CategoryQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<ICategory> GetCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(CategoryMappers.Map);

    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<PortfolioCategory> GetPortfolioCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsPortfolioCategory,
            CategoryMappers.MapGenericCategory<PortfolioCategory>
        );

    // [UseOffsetPaging]
    // [UseSorting]
    // [UseFiltering]
    // public IQueryable<ResumeCategory> GetResumeCategories([Service] IDataRepository<ICategoryEntity> repository) =>
    //     repository.Get(
    //         CategoryEntityUtils.IsResumeCategory, 
    //         CategoryMappers.MapGenericCategory<ResumeCategory>
    //     );

    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<SoftwareDevelopmentCategory> GetSoftwareDevelopmentCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsSoftwareDevelopmentCategory,
            CategoryMappers.MapGenericTechnologyCategory<SoftwareDevelopmentCategory>
        );

    // [UseOffsetPaging]
    // [UseSorting]
    // [UseFiltering]
    // public IQueryable<InformationTechnologyCategory> GetInformationTechnologyCategories([Service] IDataRepository<ICategoryEntity> repository) =>
    //     repository.Get(
    //         CategoryEntityUtils.IsInformationTechnologyCategory, 
    //         CategoryMappers.MapGenericTechnologyCategory<InformationTechnologyCategory>
    //     );

    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<ITechnologyCategory> GetTechnologyCategories([Service] IDataRepository<ICategoryEntity> repository) =>
        repository.Get(
            CategoryEntityUtils.IsTechnologyCategory,
            CategoryMappers.MapTechnologyCategories
        );

    // [UseOffsetPaging]
    // [UseSorting]
    // [UseFiltering]
    // public IQueryable<OtherCategory> GetOtherCategories([Service] IDataRepository<ICategoryEntity> repository) =>
    //     repository.Get(
    //         CategoryEntityUtils.IsIOtherCategory, 
    //         CategoryMappers.MapGenericCategory<OtherCategory>
    //     );
}
