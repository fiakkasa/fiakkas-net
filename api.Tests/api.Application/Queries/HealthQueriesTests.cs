using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Queries;

public class HealthQueriesTests
{
    [Fact]
    public async Task GetHealth_Should_Return_Data()
    {
        var service = Substitute.For<HealthCheckService>();
        service
            .CheckHealthAsync(Arg.Any<CancellationToken>())
            .Returns(
                new HealthReport(
                    entries: new Dictionary<string, HealthReportEntry>(),
                    status: HealthStatus.Healthy,
                    totalDuration: TimeSpan.FromSeconds(10)
                )
            );
        var qut = new HealthQueries();

        var result = await qut.GetHealth(service, CancellationToken.None);

        result.Status.Should().Be(HealthStatus.Healthy);
        result.MatchSnapshot();
    }
}
