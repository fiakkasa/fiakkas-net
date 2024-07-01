using api.Application.Models;

namespace api.Application.TypeExtensions;

[ExtendObjectType<SystemInfoItem>]
public sealed class SystemInfoItemTypeExtension
{
    public async ValueTask<HealthReportSummary> GetHealth(
        [Service] HealthCheckService healthCheckService,
        CancellationToken cancellationToken
    ) =>
        await healthCheckService.CheckHealthAsync(cancellationToken);
}
