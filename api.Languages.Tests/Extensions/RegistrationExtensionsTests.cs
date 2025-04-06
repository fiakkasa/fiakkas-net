using api.Languages.Extensions;
using api.Languages.Interfaces;
using api.Languages.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.Languages.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiLanguages_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new LanguagesDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(configuration)
                .AddLogging()
                .AddApiLanguages()
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<ILanguage>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<LanguagesDataConfig>>();

        Assert.NotNull(dataRepository);
        Assert.NotNull(options);
    }

    [Fact]
    public async Task AddApiLanguages_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ILanguage>>())
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiLanguages()
                .BuildSchemaAsync();

        var schema = result.Print();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();
    }
}
