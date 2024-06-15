using api.Achievements.Interfaces;

namespace api.Achievements.Models;

[ExcludeFromCodeCoverage]
public record Achievement : BaseData, IAchievement
{
    public string Content { get; init; } = string.Empty;
    public int[] Years { get; init; } = [];
}
