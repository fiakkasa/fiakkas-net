using api.Application.Models;
using api.Application.Queries;

namespace api.Application.Tests.Queries;

public class SystemQueriesQueriesTests
{
    [Fact]
    public void GetSystemStatus_Should_Return_Data()
    {
        var item = new SystemInfoItem("Version", DateTimeOffset.Now.AddMinutes(-1));

        var result = SystemQueries.GetSystemStatus(item);

        result.Version.Should().Be(item.Version);
        result.StartupTime.Should().Be(item.StartupTime);
        result.UpTime.Should().BeGreaterThan(TimeSpan.Zero);
    }
}
