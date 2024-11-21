using HotChocolate.Execution;

namespace api.Tests.Graph;

public partial class SoftwareDevelopmentCategoriesTests
{
    public class OtherCategoriesTests(GraphFixture fixture) : IClassFixture<GraphFixture>
    {
        [Fact]
        public async Task OtherCategories_Should_Return_Data()
        {
            var executor = await fixture.GetRequestExecutor();

            var result = await executor.ExecuteAsync(
                """
                {
                  otherCategories {
                    totalCount
                    nodes {
                      createdAt
                      id
                      internalId
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
        public async Task OtherCategoryById_Should_Return_Data()
        {
            var executor = await fixture.GetRequestExecutor();

            var result = await executor.ExecuteAsync(
                """
                {
                  otherCategoryById(id: "T3RoZXJDYXRlZ29yeQpnOWZkOTFmOGEyYTQ0NDUyMGE1YjE4YWExYzNjMjkxMzM=") {
                    createdAt
                    id
                    internalId
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
}
