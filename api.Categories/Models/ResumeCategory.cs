using api.Categories.Enums;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record ResumeCategory : CategoryBase
{
    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
