using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.TypeExtensions;

public class SystemInfoItemTypeExtensionTests
{
    [Fact]
    public async Task GetHealth_Should_Return_Health_Data()
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
        var teut = new SystemInfoItemTypeExtension();

        var result = await teut.GetHealth(healthCheckService, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.MatchSnapshot();
    }

}
