namespace api.Application.Models;

public sealed record HealthReportItem
{
    public string? Description { get; init; }
    public TimeSpan Duration { get; init; }
    public HealthStatus Status { get; init; }
    public IEnumerable<string>? Tags { get; init; }

    public static implicit operator HealthReportItem(HealthReportEntry entry) =>
        new()
        {
            Description = entry.Description,
            Duration = entry.Duration,
            Status = entry.Status,
            Tags = entry.Tags
        };
}
