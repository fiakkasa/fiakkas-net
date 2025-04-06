using HotChocolate.Execution;

namespace api.Tests.Graph;

public class HealthTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Health_Should_Return_Data()
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

        var fn = result.ExpectOperationResult;

        Assert.Null(fn().Errors);
        result.ToJson().MatchSnapshot();
    }
}
