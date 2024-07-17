namespace api.Application.Models.Tests;

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
            new Exception("Splash!"),
            new Dictionary<string, object>(),
            ["tag"]
        );

        var result = (HealthReportItem)healthReportEntry;

        result.Status.Should().Be(healthReportEntry.Status);
        result.Duration.Should().Be(healthReportEntry.Duration);
        result.Description.Should().Be(healthReportEntry.Description);
        result.Tags.Should().BeEquivalentTo(healthReportEntry.Tags);

        result.MatchSnapshot();
    }
}
