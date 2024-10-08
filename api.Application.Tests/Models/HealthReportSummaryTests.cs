using api.Application.Models;

namespace api.Application.Tests.Models;

public class HealthReportSummaryTests
{
    [Fact]
    public void Should_Convert_HealthReport_To_HealthReportSummary()
    {
        var duration = TimeSpan.FromSeconds(1);
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Key"] = new(
                    HealthStatus.Healthy,
                    default,
                    duration,
                    default,
                    default
                )
            },
            duration
        );

        var result = (HealthReportSummary)healthReport;

        result.Status.Should().Be(healthReport.Status);
        result.TotalDuration.Should().Be(healthReport.TotalDuration);
        result.Entries.Should().NotBeNull();
        result.Entries.Should().ContainSingle();
        result.Entries.Should().ContainKey("Key");
        result.Entries!["Key"].Status.Should().Be(healthReport.Entries["Key"].Status);
        result.Entries!["Key"].Duration.Should().Be(healthReport.Entries["Key"].Duration);

        result.MatchSnapshot();
    }
}
