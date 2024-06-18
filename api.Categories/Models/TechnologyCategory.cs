using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record TechnologyCategory : Category, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
