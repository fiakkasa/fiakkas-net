using GraphHealthChecks;
using HealthChecks.ApplicationStatus.DependencyInjection;

namespace api.Extensions;

public static class HealthExtensions
{
    public static IHealthChecksBuilder AddApiHealth(this IServiceCollection services) =>
        services
            .AddHealthChecks()
            .AddApplicationStatus(name: Consts.ApiHealthName)
            .AddGraphHealthWithILogger(healthName: Consts.GraphQLHealthName);
}
