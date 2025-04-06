using HotChocolate.Execution;

namespace api.Tests.Graph;

public class LanguageTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Languages_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              languages {
                totalCount
                nodes {
                  createdAt
                  id
                  internalId
                  proficiency
                  title
                  updatedAt
                  version
                }
              }
            }
            """);

        var fn = result.ExpectOperationResult;

        Assert.Null(fn().Errors);
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task LanguageById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              languageById(id: "TGFuZ3VhZ2UKZzAyYTNiZTliM2YwNDRiNGE4OTQ1ZTg0ZmVmNTM3YjU4") {
                createdAt
                id
                internalId
                proficiency
                title
                updatedAt
                version
              }
            }
            """);

        var fn = result.ExpectOperationResult;

        Assert.Null(fn().Errors);
        result.ToJson().MatchSnapshot();
    }
}
