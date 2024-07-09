using api.Achievements.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Achievements.Services.Tests;

public class AchievementDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new AchievementEntity
        {
            Id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2024]
        };
        var configData = new AchievementsDataConfig
        {
            Achievements = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<AchievementsDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new AchievementDataRepository(Substitute.For<ILogger<AchievementDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }
}
