namespace api.Shared.Types.Interfaces;

public interface IBaseData : IBaseId
{
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset? UpdatedAt { get; init; }
    long Version { get; init; }
}
