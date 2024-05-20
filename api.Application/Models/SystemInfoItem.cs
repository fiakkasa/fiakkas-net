namespace api.Application.Models;

public sealed record SystemInfoItem(string Version, DateTimeOffset StartupTime)
{
    public TimeSpan UpTime => (DateTimeOffset.Now - StartupTime) switch
    {
        { TotalMilliseconds: > 0 } ts => ts,
        _ => TimeSpan.Zero
    };
}
