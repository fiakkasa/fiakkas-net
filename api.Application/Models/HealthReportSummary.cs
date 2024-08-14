namespace api.Application.Models;

public sealed record HealthReportSummary
{
    public HealthStatus Status { get; init; }
    public TimeSpan TotalDuration { get; init; }
    public IReadOnlyDictionary<string, HealthReportItem>? Entries { get; init; }

    public static implicit operator HealthReportSummary(HealthReport healthReport) =>
        new()
        {
            Status = healthReport.Status,
            TotalDuration = healthReport.TotalDuration,
            Entries = healthReport.Entries.ToDictionary(x => x.Key, x => (HealthReportItem)x.Value)
        };
}
