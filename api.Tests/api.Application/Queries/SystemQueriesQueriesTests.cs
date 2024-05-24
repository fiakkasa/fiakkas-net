using api.Application.Models;

namespace api.Application.Queries;

public class SystemQueriesQueriesTests
{
    [Fact]
    public void GetSystemStatus_Should_Return_Data()
    {
        var item = new SystemInfoItem("Version", DateTimeOffset.Now.AddMinutes(-1));
        var qut = new SystemQueries();

        var result = qut.GetSystemStatus(item);

        result.Version.Should().Be(item.Version);
        result.StartupTime.Should().Be(item.StartupTime);
        result.UpTime.Should().BeGreaterThan(TimeSpan.Zero);
    }
}
