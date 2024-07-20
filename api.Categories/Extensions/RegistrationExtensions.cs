using api.Categories.DataLoaders;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.Categories.Queries;
using api.Categories.Services;
using api.Categories.TypeExtensions;

namespace api.Categories.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiCategories(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services.AddBoundOptions<CategoriesDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<ICategoryEntity>, CategoryDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiCategories(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<AssociatedCategoryGroupDataLoader>()
            .AddDataLoader<InformationTechnologyCategoryBatchDataLoader>()
            .AddDataLoader<OtherCategoryBatchDataLoader>()
            .AddDataLoader<PortfolioCategoryBatchDataLoader>()
            .AddDataLoader<ResumeCategoryBatchDataLoader>()
            .AddDataLoader<SoftwareDevelopmentCategoryBatchDataLoader>()
            .AddDataLoader<UnknownCategoryBatchDataLoader>()
            .AddObjectType<InformationTechnologyCategory>()
            .AddObjectType<OtherCategory>()
            .AddObjectType<PortfolioCategory>()
            .AddObjectType<ResumeCategory>()
            .AddObjectType<SoftwareDevelopmentCategory>()
            .AddObjectType<UnknownCategory>()
            .AddObjectType<UnknownTechnologyCategory>()
            .AddTypeExtension(typeof(CategoryQueries))
            .AddTypeExtension<ResumeCategoryTypeExtension>();
}
