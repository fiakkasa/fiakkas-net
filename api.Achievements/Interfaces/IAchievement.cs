namespace api.Achievements.Interfaces;

public interface IAchievement : IBaseData
{
    string Content { get; init; }
    int[] Years { get; init; }
}
