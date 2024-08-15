namespace api.Extensions;

public static class HealthExtensions
{
    public static IHealthChecksBuilder AddApiHealth(this IServiceCollection services) =>
        services
            .AddHealthChecks()
            .AddApplicationStatus(Consts.ApiHealthName)
            .AddGraphHealthWithILogger(Consts.GraphQLHealthName);
}
