namespace api.Shared.Interfaces;

public interface IBaseData
{
    Guid Id { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset? UpdatedAt { get; init; }
    long Version { get; init; }
}
