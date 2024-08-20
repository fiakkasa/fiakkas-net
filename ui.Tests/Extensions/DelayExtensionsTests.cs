using FluentAssertions.Extensions;
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

        var func = () => delay.SafeDelay(cts.Token).AsTask();
        
        await func.Should().CompleteWithinAsync(maxExecutionTime.Milliseconds());
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
        
        var func = () => TimeSpan.FromMilliseconds(delay).SafeDelay(cts.Token).AsTask();

        await func.Should().CompleteWithinAsync(maxExecutionTime.Milliseconds());
    }
}
