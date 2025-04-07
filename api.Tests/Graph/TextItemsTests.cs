using HotChocolate.Execution;

namespace api.Tests.Graph;

public class TextItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task TextItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              textItems {
                totalCount
                nodes {
                  content
                  createdAt
                  id
                  internalId
                  key
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
    public async Task TextItemById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              textItemById(id: "VGV4dEl0ZW0KZzQ4ZTQ4M2U0Njk2MTRiMjU4OGE5ZDFkMGE1MTYxMTA5") {
                content
                createdAt
                id
                internalId
                key
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
