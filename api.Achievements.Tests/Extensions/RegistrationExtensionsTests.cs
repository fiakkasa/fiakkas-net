using api.Achievements.Extensions;
using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.Achievements.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiAchievements_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new AchievementsDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(configuration)
                .AddLogging()
                .AddApiAchievements()
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<IAchievement>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<AchievementsDataConfig>>();

        Assert.NotNull(dataRepository);
        Assert.NotNull(options);
    }

    [Fact]
    public async Task AddApiAchievements_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IAchievement>>())
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiAchievements()
                .BuildSchemaAsync();

        var schema = result.Print();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();
    }
}
