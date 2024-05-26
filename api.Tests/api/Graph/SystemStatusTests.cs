using HotChocolate.Execution;

namespace api.Tests;

public class SystemStatusTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task SystemStatus_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
"""
{
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
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}

