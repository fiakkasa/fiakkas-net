using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record TechnologyCategoryBase : AbstractCategory, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
