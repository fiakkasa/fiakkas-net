using api.Application.Models;

namespace api.Application.Tests.Models;

public class HealthReportItemTests
{
    [Fact]
    public void Should_Convert_HealthReportItem_To_HealthReportEntry()
    {
        var duration = TimeSpan.FromSeconds(1);
        var healthReportEntry = new HealthReportEntry(
            HealthStatus.Healthy,
            "description",
            duration,
            new("Splash!"),
            new Dictionary<string, object>(),
            ["tag"]
        );

        var result = (HealthReportItem)healthReportEntry;


        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Equivalent(duration, result.Duration, true);
        Assert.Equal(healthReportEntry.Description, result.Description);
        Assert.Equal(1, result.Tags?.Count());
        Assert.Equal(healthReportEntry.Tags.First(), result.Tags?.FirstOrDefault());

        result.MatchSnapshot();
    }
}
