using api.Application.TypeExtensions;

namespace api.Application.Tests.TypeExtensions;

public class SystemInfoItemTypeExtensionTests
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
        var sut = new SystemInfoItemTypeExtension();

        var result = await sut.GetHealth(service, CancellationToken.None);

        Assert.Equal(HealthStatus.Healthy, result.Status);
        result.MatchSnapshot();
    }
}
