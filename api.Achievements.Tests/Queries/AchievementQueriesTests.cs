using api.Achievements.DataLoaders;
using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Achievements.Queries;
using GreenDonut;

namespace api.Achievements.Tests.Queries;

public class AchievementQueriesTests
{
    [Fact]
    public void GetAchievements_Should_Return_Data()
    {
        var item = new Achievement
        {
            Id = new("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2024]
        };
        var dataRepository = new MockDataRepository<IAchievement>([item]);

        var result = AchievementQueries.GetAchievements(dataRepository);

        Assert.Single(result);
        Assert.IsAssignableFrom<IQueryable<Achievement>>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetAchievementById_Should_Return_Data_When_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var item = new Achievement
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2024]
        };
        var dataRepository = new MockDataRepository<IAchievement>([item]);
        var dataLoader = new AchievementBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await AchievementQueries.GetAchievementById(
            id,
            dataLoader,
            default
        );

        Assert.NotNull(result);
        Assert.IsType<Achievement>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetAchievementById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add");
        var dataRepository = new MockDataRepository<IAchievement>([]);
        var dataLoader = new AchievementBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await AchievementQueries.GetAchievementById(
            id,
            dataLoader,
            default
        );

        Assert.Null(result);
    }
}
