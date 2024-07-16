using api.Achievements.Interfaces;
using api.Achievements.Models;

namespace api.Achievements.Queries.Tests;

public class AchievementQueriesTests
{
    [Fact]
    public void GetAchievements_Should_Return_Data()
    {
        var item = new Achievement
        {
            Id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2024]
        };
        var dataRepository = new MockDataRepository<IAchievement>([item]);

        var result = AchievementQueries.GetAchievements(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<Achievement>>();
        result.MatchSnapshot();
    }
}
