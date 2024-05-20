using api.Application.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class HealthQueries
{
    public async ValueTask<HealthReportSummary> GetHealth(
        [Service] HealthCheckService healthCheckService,
        CancellationToken cancellationToken
    ) =>
        (await healthCheckService.CheckHealthAsync(cancellationToken)).Adapt<HealthReportSummary>();
}
