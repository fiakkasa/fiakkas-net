using api.Application.Models;
using HotChocolate.Execution;

namespace api.Application.Extensions.Tests;

public class RegistrationExtensionsTests
{
    private static readonly DateTimeOffset _start = DateTimeOffset.Now;

    [Fact]
    public void AddApiApplication_Service_Registration_Should_Add_SystemInfo_With_Empty_Version_When_Version_Does_Not_Resolve()
    {
        var result =
            new ServiceCollection()
                .AddApiApplication(_start)
                .BuildServiceProvider()
                .GetService<SystemInfoItem>();

        result.Should().NotBeNull();
        result!.Version.Should().BeEmpty();
    }

    [Fact]
    public void AddApiApplication_Service_Registration_Should_Add_SystemInfo_With_Version()
    {
        var value = "1.1.1";
        var result =
            new ServiceCollection()
                .AddApiApplication(_start, new(value))
                .BuildServiceProvider()
                .GetService<SystemInfoItem>();

        result.Should().NotBeNull();
        result!.Version.Should().Be(value);
    }

    [Fact]
    public async Task AddApiApplication_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddGraphQL()
                .AddQueryType()
                .AddApiApplication()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
