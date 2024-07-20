namespace api.Categories.Interfaces;

public interface ICategory 
{
    Guid Id { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset? UpdatedAt { get; init; }
    long Version { get; init; }
    string Title { get; init; }
}
