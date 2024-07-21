using api.Categories.DataLoaders;
using api.Categories.Interfaces;
using api.Categories.Mappers;
using api.Categories.Models;
using api.Categories.Utils;
using HotChocolate.Types.Relay;

namespace api.Categories.Queries;

[QueryType]
public static class CategoryQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Category> GetCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(CategoryMappers.MapCategory);
    
    [NodeResolver]
    public static async ValueTask<Category?> GetCategoryById(
        Guid id,
        [Service] CategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<PortfolioCategory> GetPortfolioCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsPortfolioCategory,
            CategoryMappers.MapPortfolioCategory
        );

    [NodeResolver]
    public static async ValueTask<PortfolioCategory?> GetPortfolioCategoryById(
        Guid id,
        [Service] PortfolioCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

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
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<SoftwareDevelopmentCategory> GetSoftwareDevelopmentCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsSoftwareDevelopmentCategory,
            CategoryMappers.MapSoftwareDevelopmentCategory
        );

    [NodeResolver]
    public static async ValueTask<SoftwareDevelopmentCategory?> GetSoftwareDevelopmentCategoryById(
        Guid id,
        [Service] SoftwareDevelopmentCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<InformationTechnologyCategory> GetInformationTechnologyCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsInformationTechnologyCategory,
            CategoryMappers.MapInformationTechnologyCategory
        );

    
    [NodeResolver]
    public static async ValueTask<InformationTechnologyCategory?> GetInformationTechnologyCategoryById(
        Guid id,
        [Service] InformationTechnologyCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<TechnologyCategory> GetTechnologyCategories([Service] IDataRepository<ICategory> repository) =>
        repository.Get(
            CategoryEntityUtils.IsTechnologyCategory,
            CategoryMappers.MapTechnologyCategory
        );

    [NodeResolver]
    public static async ValueTask<TechnologyCategory?> GetTechnologyCategoryById(
        Guid id,
        [Service] TechnologyCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);

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
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
