using HotChocolate.Execution;

namespace api.Application.Tests;

public class HealthTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Health_Should_Return_Results()
    {
        var executor = await fixture.GetRequestExecutor();

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
}
""");

        Func<IQueryResult> fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        result.ToJson().MatchSnapshot();
    }
}
