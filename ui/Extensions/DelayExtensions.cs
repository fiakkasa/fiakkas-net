namespace ui.Extensions;

public static class DelayExtensions
{
    public static async ValueTask SafeDelay(this TimeSpan delay, CancellationToken cancellationToken = default)
    {
        try
        {
            await (delay switch
            {
                { TotalMilliseconds: > 0 } => Task.Delay(delay, cancellationToken),
                _ => Task.CompletedTask
            });
        }
        catch { /* ignore */ }
    }

    public static async ValueTask SafeDelay(this int millisecondsDelay, CancellationToken cancellationToken = default)
    {
        try
        {
            await (millisecondsDelay switch
            {
                > 0 => Task.Delay(millisecondsDelay, cancellationToken),
                _ => Task.CompletedTask
            });
        }
        catch { /* ignore */ }
    }
}
