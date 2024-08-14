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
    public static IQueryable<IPolymorphicCategory> GetCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(CategoryMappers.MapPolymorphicCategory);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<UnknownCategory> GetUnknownCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(CategoryEntityUtils.IsUnknownCategory, CategoryMappers.MapUnknownCategory);

    [NodeResolver]
    public static async ValueTask<UnknownCategory?> GetUnknownCategoryById(
        Guid id,
        [Service] UnknownCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<PortfolioCategory> GetPortfolioCategories(
        [Service] IDataRepository<ICategory> repository
    ) => repository.Get(
        CategoryEntityUtils.IsPortfolioCategory,
        CategoryMappers.MapPortfolioCategory
    );

    [NodeResolver]
    public static async ValueTask<PortfolioCategory?> GetPortfolioCategoryById(
        Guid id,
        [Service] PortfolioCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<ResumeCategory> GetResumeCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsResumeCategory,
            CategoryMappers.MapResumeCategory
        );

    [NodeResolver]
    public static async ValueTask<ResumeCategory?> GetResumeCategoryById(
        Guid id,
        [Service] ResumeCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<SoftwareDevelopmentCategory> GetSoftwareDevelopmentCategories(
        [Service] IDataRepository<ICategory> repository
    ) => repository.Get(
        CategoryEntityUtils.IsSoftwareDevelopmentCategory,
        CategoryMappers.MapSoftwareDevelopmentCategory
    );

    [NodeResolver]
    public static async ValueTask<SoftwareDevelopmentCategory?> GetSoftwareDevelopmentCategoryById(
        Guid id,
        [Service] SoftwareDevelopmentCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<InformationTechnologyCategory> GetInformationTechnologyCategories(
        [Service] IDataRepository<ICategory> repository
    ) => repository.Get(
        CategoryEntityUtils.IsInformationTechnologyCategory,
        CategoryMappers.MapInformationTechnologyCategory
    );

    [NodeResolver]
    public static async ValueTask<InformationTechnologyCategory?> GetInformationTechnologyCategoryById(
        Guid id,
        [Service] InformationTechnologyCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<IPolymorphicTechnologyCategory> GetTechnologyCategories(
        [Service] IDataRepository<ICategory> repository
    ) => repository.Get(
        CategoryEntityUtils.IsTechnologyCategory,
        CategoryMappers.MapPolymorphicTechnologyCategory
    );

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<OtherCategory> GetOtherCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsOtherCategory,
            CategoryMappers.MapOtherCategory
        );

    [NodeResolver]
    public static async ValueTask<OtherCategory?> GetOtherCategoryById(
        Guid id,
        [Service] OtherCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) => await dataLoader.LoadAsync(id, cancellationToken);
}
