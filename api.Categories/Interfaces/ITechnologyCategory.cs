namespace api.Categories.Interfaces;

public interface ITechnologyCategory : ICategoryBase
{
    new Guid Id { get; init; }
    new DateTimeOffset CreatedAt { get; init; }
    new DateTimeOffset? UpdatedAt { get; init; }
    new long Version { get; init; }
    new string Title { get; init; }
    Uri? Href { get; init; }
}
