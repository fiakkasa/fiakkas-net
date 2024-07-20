using api.Categories.Enums;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record ResumeCategory : AbstractCategory
{
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
