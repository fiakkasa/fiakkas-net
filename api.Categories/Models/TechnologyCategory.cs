using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record TechnologyCategory : AbstractCategoryBase, ITechnologyCategory, ICategoryKind
{
    public CategoryType Kind { get; init; }
    public Uri? Href { get; init; }
}
