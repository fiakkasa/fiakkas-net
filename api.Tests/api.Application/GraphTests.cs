using api.Application.Extensions;
using api.Application.Models;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Tests;

public class GraphTests
{
    [Fact]
    public async Task Graph_Should_Return_Results()
    {
        var systemInfoItem = new SystemInfoItem("Version", new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var healthCheckService = Substitute.For<HealthCheckService>();

        healthCheckService
            .CheckHealthAsync(Arg.Any<CancellationToken>())
            .Returns(
                new HealthReport(
                    entries: new Dictionary<string, HealthReportEntry>(),
                    status: HealthStatus.Healthy,
                    totalDuration: TimeSpan.FromSeconds(10)
                )
            );
        var executor =
            await new ServiceCollection()
                .AddSingleton(systemInfoItem)
                .AddSingleton(healthCheckService)
                .AddGraphQL()
                .AddQueryType()
                .AddApiApplication()
                .BuildRequestExecutorAsync();

        var result = await executor.ExecuteAsync(
"""
{
  health {
    status
    totalDuration
    entries {
      key
      value {
        description
        duration
        status
        tags
      }
    }
  }
  systemStatus {
    startupTime
    # omit upTime as it varies
    #upTime
    version
    health {
      status
      totalDuration
      entries {
        key
        value {
          description
          duration
          status
          tags
        }
      }
    }
  }
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        result.ToJson().MatchSnapshot();
    }
}
