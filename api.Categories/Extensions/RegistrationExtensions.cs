using api.Categories.Interfaces;
using api.Categories.Models;
using api.Categories.Queries;
using api.Categories.Services;

namespace api.Categories.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiCategories(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<CategoriesDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<ICategoryEntity>, CategoryDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiCategories(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension<CategoryQueries>()
            .AddObjectType<Category>()
            // .AddObjectType<InformationTechnologyCategory>()
            .AddObjectType<PortfolioCategory>()
            .AddObjectType<ResumeCategory>()
            .AddObjectType<SoftwareDevelopmentCategory>()
            .AddObjectType<TechnologyCategory>();
}
