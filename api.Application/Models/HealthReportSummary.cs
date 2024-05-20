using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Models;

[ExcludeFromCodeCoverage]
public sealed record HealthReportSummary
{
    public HealthStatus Status { get; init; }
    public TimeSpan TotalDuration { get; init; }
    public IReadOnlyDictionary<string, HealthReportItem>? Entries { get; init; }
}
