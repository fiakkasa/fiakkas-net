using api.Categories.Enums;

namespace api.Categories.Interfaces;

public interface ICategoryKind
{
    CategoryType Kind { get; init; }
}
public interface ICategoryTitle
{
    string Title { get; init; }
}
public interface ICategoryUri
{
    Uri? Href { get; init; }
}
public interface ICategoryAssociatedCategoryTypes
{
    CategoryType[] AssociatedCategoryTypes { get; init; }
}

public interface ICategory :
    IBaseData,
    ICategoryKind,
    ICategoryTitle,
    ICategoryUri,
    ICategoryAssociatedCategoryTypes
{
}
