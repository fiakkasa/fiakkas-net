using System.Diagnostics;
using System.Threading;
using ui.Extensions;

namespace ui.Tests.Extensions;

public class DelayExtensionsTests
{
    [Theory]
    [InlineData(250, 500, 1000)]
    [InlineData(-1, 250, 1000)]
    [InlineData(1000, 500, 250)]
    public async Task SafeDelayOfInt_Should_Delay_The_Task_By_The_Specified_Delay(
        int delay,
        int maxExecutionTime,
        int cancelAfter
    )
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(cancelAfter);

        var start = Stopwatch.GetTimestamp();

        await delay.SafeDelay(cts.Token);

        Assert.True(maxExecutionTime > Stopwatch.GetElapsedTime(start).TotalMilliseconds);
    }

    [Theory]
    [InlineData(250, 500, 1000)]
    [InlineData(-1, 250, 1000)]
    [InlineData(1000, 500, 250)]
    public async Task SafeDelayOfTimeSpan_Should_Delay_The_Task_By_The_Specified_Delay(
        int delay,
        int maxExecutionTime,
        int cancelAfter
    )
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(cancelAfter);

        var start = Stopwatch.GetTimestamp();

        await TimeSpan.FromMilliseconds(delay).SafeDelay(cts.Token);

        Assert.True(maxExecutionTime > Stopwatch.GetElapsedTime(start).TotalMilliseconds);
    }
}
