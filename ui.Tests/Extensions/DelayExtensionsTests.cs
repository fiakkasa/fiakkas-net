using System.Diagnostics;
using System.Threading;

namespace ui.Extensions.Tests;

public class DelayExtensionsTests
{
    [Theory]
    [InlineData(250, 249, 260, 1000)]
    [InlineData(-1, 0, 3, 1000)]
    [InlineData(1000, 249, 260, 250)]
    public async Task SafeDelayOfInt_Should_Delay_The_Task_By_The_Specified_Delay(int delay, int startRange, int endRange, int cancelAfter)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(cancelAfter);
        var start = Stopwatch.GetTimestamp();

        await delay.SafeDelay(cts.Token);

        var result = Stopwatch.GetElapsedTime(start);

        result.Milliseconds.Should().BeInRange(startRange, endRange);
    }

    [Theory]
    [InlineData(250, 249, 260, 1000)]
    [InlineData(-1, 0, 3, 1000)]
    [InlineData(1000, 249, 260, 250)]
    public async Task SafeDelayOfTimeSpan_Should_Delay_The_Task_By_The_Specified_Delay(int delay, int startRange, int endRange, int cancelAfter)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(cancelAfter);
        var start = Stopwatch.GetTimestamp();

        await TimeSpan.FromMilliseconds(delay).SafeDelay(cts.Token);

        var result = Stopwatch.GetElapsedTime(start);

        result.Milliseconds.Should().BeInRange(startRange, endRange);
    }
}
