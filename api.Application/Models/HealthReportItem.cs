using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Models;

[ExcludeFromCodeCoverage]
[AdaptFrom(typeof(HealthReportEntry))]
public sealed record HealthReportItem
{
    public string? Description { get; init; }
    public TimeSpan Duration { get; init; }
    public HealthStatus Status { get; init; }
    public IEnumerable<string>? Tags { get; init; }
}
