using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record CategoryEntity : AbstractBaseData, ICategory
{
    public CategoryType Kind { get; init; }

    public string Title { get; init; } = string.Empty;

    public Uri? Href { get; init; }

    public CategoryType[] AssociatedCategoryTypes { get; init; } = [];
}
