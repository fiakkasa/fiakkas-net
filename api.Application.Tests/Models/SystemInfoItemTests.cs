namespace api.Application.Models.Tests;

public class SystemInfoItemTests
{
    [Fact]
    public void Should_Return_Non_Zero_UpTime_When_The_Difference_Of_StartupTime_And_Now_Is_Greater_Than_Zero()
    {
        var result = new SystemInfoItem("Version", DateTimeOffset.Now.AddSeconds(-1)).UpTime;

        result.Should().BeGreaterThan(TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public void Should_Return_Zero_UpTime_When_The_Difference_Of_StartupTime_And_Now_Is_Less_Than_Or_Equal_To_Zero()
    {
        var result = new SystemInfoItem("Version", DateTimeOffset.Now.AddMinutes(1)).UpTime;

        result.Should().Be(TimeSpan.Zero);
    }
}
