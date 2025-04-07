namespace api.Tests.Graph;

public class SchemaTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task Schema_Should_Resolve()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = executor.Schema.Print();

        Assert.NotEmpty(result);
        result.MatchSnapshot();
    }
}
