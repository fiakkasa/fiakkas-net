namespace api.Categories.Interfaces;

public interface IPolymorphicCategory : IBaseId
{
    new Guid Id { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset? UpdatedAt { get; init; }
    long Version { get; init; }
    string Title { get; init; }
}
