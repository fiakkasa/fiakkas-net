using api.Shared.Types.Interfaces;
using api.TextItems.Extensions;
using api.TextItems.Interfaces;
using api.TextItems.Models;
using HotChocolate.Execution;

namespace api.TextItems.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiTextItems_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new TextItemsDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiTextItems(configuration)
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<ITextItem>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<TextItemsDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiTextItems_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ITextItem>>())
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiTextItems()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();
    }
}
