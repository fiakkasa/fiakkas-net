using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record ResumeCategory :
    AbstractCategoryBase,
    ICategoryAssociatedCategoryTypes,
    IPolymorphicCategory
{
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
