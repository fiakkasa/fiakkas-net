using api.EducationItems.DataLoaders;
using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.EducationItems.Queries;
using api.EducationItems.Services;

namespace api.EducationItems.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiEducationItems(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services.AddBoundOptions<EducationItemsDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<IEducationItem>, EducationItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiEducationItems(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<EducationItemBatchDataLoader>()
            .AddTypeExtension(typeof(EducationItemQueries));
}
