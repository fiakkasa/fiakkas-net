using api.Categories.Enums;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record ResumeCategory : AbstractCategoryBase
{
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
