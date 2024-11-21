using HotChocolate.Execution;

namespace api.Tests.Graph;

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

        var fn = result.ExpectOperationResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
