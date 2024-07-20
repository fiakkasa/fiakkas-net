namespace api.Categories.Interfaces;

public interface ITechnologyCategory
{
    Guid Id { get; init; }
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset? UpdatedAt { get; init; }
    long Version { get; init; }
    string Title { get; init; }
    Uri? Href { get; init; }
}
