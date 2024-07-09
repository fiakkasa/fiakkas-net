using api.Categories.Enums;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record ResumeCategory : Category
{
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
