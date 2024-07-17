using api.Shared.Types.Interfaces;

namespace api.Shared.Types.Models;

[ExcludeFromCodeCoverage]
public abstract record BaseData : IBaseData
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public long Version { get; init; }
}
