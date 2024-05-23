using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Queries;

public class HealthQueriesTests
{
    [Fact]
    public async Task GetHealth_Should_Return_Result()
    {
        var healthCheckService = Substitute.For<HealthCheckService>();

        healthCheckService
            .CheckHealthAsync(Arg.Any<CancellationToken>())
            .Returns(
                new HealthReport(
                    entries: new Dictionary<string, HealthReportEntry>(),
                    status: HealthStatus.Healthy,
                    totalDuration: TimeSpan.FromSeconds(10)
                )
            );
        var qut = new HealthQueries();

        var result = await qut.GetHealth(healthCheckService, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.MatchSnapshot();
    }
}
