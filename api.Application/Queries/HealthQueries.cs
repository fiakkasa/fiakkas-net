using api.Application.Models;

namespace api.Application.Queries;

[QueryType]
public sealed class HealthQueries
{
    public async ValueTask<HealthReportSummary> GetHealth(
        [Service] HealthCheckService healthCheckService,
        CancellationToken cancellationToken
    ) =>
        await healthCheckService.CheckHealthAsync(cancellationToken);
}
