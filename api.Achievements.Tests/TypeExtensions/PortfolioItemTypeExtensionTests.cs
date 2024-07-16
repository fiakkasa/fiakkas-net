using api.Achievements.Models;

namespace api.Achievements.TypeExtensions.Tests;

public class AchievementTypeExtensionTests
{
    [Fact]
    public void GetYearsSummary_Should_Return_Content()
    {
        var item = new Achievement
        {
            Id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Content = "Content",
            Years = [2022, 2023, 2024]
        };
        var sut = new AchievementTypeExtension();

        var result = sut.GetYearsSummary(item);

        result.Should().Be("2022, 2023, 2024");
        result.MatchSnapshot();
    }
}
