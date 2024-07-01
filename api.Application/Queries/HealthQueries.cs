using api.Application.Models;

namespace api.Application.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class HealthQueries
{
    public async ValueTask<HealthReportSummary> GetHealth(
        [Service] HealthCheckService healthCheckService,
        CancellationToken cancellationToken
    ) =>
        await healthCheckService.CheckHealthAsync(cancellationToken);
}
