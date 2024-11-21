using HotChocolate.Execution;

namespace api.Tests.Graph;

public class ContactItemsTests(GraphFixture fixture) : IClassFixture<GraphFixture>
{
    [Fact]
    public async Task ContactItems_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              contactItems {
                totalCount
                nodes {
                  createdAt
                  description
                  href
                  icon
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
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task ContactItemById_Should_Return_Data()
    {
        var executor = await fixture.GetRequestExecutor();

        var result = await executor.ExecuteAsync(
            """
            {
              contactItemById(id: "Q29udGFjdEl0ZW0KZ2ViZjIyNGE4N2ZmMzQ3Yjk4ODJiZGQ0MWVjN2Y1YTA1") {
                createdAt
                description
                href
                icon
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
        fn.Should().NotThrow();
        fn().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }
}
