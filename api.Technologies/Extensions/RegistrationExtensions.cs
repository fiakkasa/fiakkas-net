using api.Technologies.Interfaces;
using api.Technologies.Models;
using api.Technologies.Queries;
using api.Technologies.Services;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.Technologies.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiTechnologies(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<TechnologyDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<ITechnology>, TechnologyDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiTechnologies(this IRequestExecutorBuilder builder) =>
        builder.AddTypeExtension<TechnologyQueries>();
}
