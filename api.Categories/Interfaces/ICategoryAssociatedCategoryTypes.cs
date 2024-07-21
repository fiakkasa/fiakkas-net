using api.Categories.Enums;

namespace api.Categories.Interfaces;

public interface ICategoryAssociatedCategoryTypes
{
    CategoryType[] AssociatedCategoryTypes { get; init; }
}
