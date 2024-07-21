using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record Category : AbstractCategoryBase, ICategory
{
    public CategoryType Kind { get; init; }
    public Uri? Href { get; init; }
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
