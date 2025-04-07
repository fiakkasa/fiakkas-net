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

        Assert.Equal(healthReport.Status, result.Status);
        Assert.Equivalent(healthReport.TotalDuration, result.TotalDuration, true);
        Assert.NotNull(result.Entries);
        Assert.Single(result.Entries);
        Assert.True(result.Entries.ContainsKey("Key"));
        Assert.Equal(healthReport.Entries["Key"].Status, result.Entries["Key"].Status);
        Assert.Equivalent(healthReport.Entries["Key"].Duration, result.Entries["Key"].Duration, true);

        result.MatchSnapshot();
    }
}
