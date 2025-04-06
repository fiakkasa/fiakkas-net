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

        Assert.Equal(item.Version, result.Version);
        Assert.Equivalent(item.StartupTime, result.StartupTime, true);
        Assert.True(result.UpTime > TimeSpan.Zero);
    }
}
