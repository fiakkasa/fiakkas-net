using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.Achievements.Extensions.Tests;

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
                .AddLogging()
                .AddApiAchievements(configuration)
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<IAchievement>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<AchievementsDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiAchievements_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IAchievement>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiAchievements()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
