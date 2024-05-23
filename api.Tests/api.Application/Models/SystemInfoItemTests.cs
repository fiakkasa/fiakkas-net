namespace api.Application.Models.Tests;

public class SystemInfoItemTests
{
    [Fact]
    public void Returns_UpTime()
    {
        var result = new SystemInfoItem("Version", DateTimeOffset.Now.AddSeconds(-1)).UpTime;

        result.Should().BeGreaterThan(TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public void Returns_Zero_UpTime_When_TotalMilliseconds_Evaluate_To_Less_Than_Or_Equal_To_Zero()
    {
        var result = new SystemInfoItem("Version", DateTimeOffset.Now.AddMinutes(1)).UpTime;

        result.Should().Be(TimeSpan.Zero);
    }
}
