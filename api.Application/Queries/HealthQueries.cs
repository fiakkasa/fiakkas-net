using api.Application.Models;

namespace api.Application.Queries;

[QueryType]
public static class HealthQueries
{
    public static async ValueTask<HealthReportSummary> GetHealth(
        [Service] HealthCheckService healthCheckService,
        CancellationToken cancellationToken
    ) =>
        await healthCheckService.CheckHealthAsync(cancellationToken);
}
