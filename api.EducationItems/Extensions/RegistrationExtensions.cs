using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.EducationItems.Queries;
using api.EducationItems.Services;

namespace api.EducationItems.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiEducationItems(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<EducationItemsDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<IEducationItem<EducationTimePeriod>>, EducationItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiEducationItems(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension<EducationItemQueries>();
}
