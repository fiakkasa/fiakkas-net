using HotChocolate.Execution;

namespace api.Tests.Graph;

public class AchievementsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Achievements_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              achievements {
                totalCount
                nodes {
                  content
                  createdAt
                  id
                  internalId
                  updatedAt
                  version
                  years
                  yearsSummary
                }
              }
            }
            """);

        var fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task AchievementById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              achievementById(id: "QWNoaWV2ZW1lbnQKZ2Q0NjA1YjBjNThiYzQ5YWNiY2ZkMTBhMjRhMjAzYWRk") {
                content
                createdAt
                id
                internalId
                updatedAt
                version
                years
                yearsSummary
              }
            }
            """);

        var fn = result.ExpectQueryResult;
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
