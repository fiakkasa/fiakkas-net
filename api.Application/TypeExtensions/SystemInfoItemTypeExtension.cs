using api.Application.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
