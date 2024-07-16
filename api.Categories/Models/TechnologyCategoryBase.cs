using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record TechnologyCategoryBase : CategoryBase, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
