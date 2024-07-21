namespace api.Categories.Interfaces;

public interface ICategory :
    IBaseData,
    ICategoryKind,
    ICategoryTitle,
    ICategoryUri,
    ICategoryAssociatedCategoryTypes
{ }
