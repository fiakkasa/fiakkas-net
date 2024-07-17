using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.EducationItems.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiEducationItems_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new EducationItemsDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiEducationItems(configuration)
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<IEducationItem>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<EducationItemsDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiEducationItems_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IEducationItem>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiEducationItems()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
