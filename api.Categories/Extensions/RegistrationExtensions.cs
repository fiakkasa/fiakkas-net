using api.Categories.Interfaces;
using api.Categories.Models;
using api.Categories.Services;

namespace api.Categories.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiCategories(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services.AddBoundOptions<CategoriesDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<ICategory>, CategoryDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiCategories(this IRequestExecutorBuilder builder) =>
        builder
            .AddCategoriesGraph()
            // .AddDataLoader<AssociatedCategoryGroupDataLoader>()
            // .AddDataLoader<InformationTechnologyCategoryBatchDataLoader>()
            // .AddDataLoader<OtherCategoryBatchDataLoader>()
            // .AddDataLoader<PortfolioCategoryBatchDataLoader>()
            // .AddDataLoader<ResumeCategoryBatchDataLoader>()
            // .AddDataLoader<SoftwareDevelopmentCategoryBatchDataLoader>()
            // .AddDataLoader<UnknownCategoryBatchDataLoader>()
            .AddInterfaceType<IPolymorphicCategory>(descriptor => descriptor.Field(f => f.Id).ID())
            .AddInterfaceType<IPolymorphicTechnologyCategory>(descriptor => descriptor.Field(f => f.Id).ID())
            .AddObjectType<UnknownTechnologyCategory>(descriptor => descriptor.Field(f => f.Id).ID());
    // .AddTypeExtension(typeof(CategoryQueries))
    // .AddTypeExtension<ICategoryAssociatedCategoryTypesTypeExtension>();
}
