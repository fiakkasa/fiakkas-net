using api.Application.Queries;

namespace api.Application.Tests.Queries;

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
                    new Dictionary<string, HealthReportEntry>(),
                    HealthStatus.Healthy,
                    TimeSpan.FromSeconds(10)
                )
            );

        var result = await HealthQueries.GetHealth(service, CancellationToken.None);

        result.Status.Should().Be(HealthStatus.Healthy);
        result.MatchSnapshot();
    }
}
