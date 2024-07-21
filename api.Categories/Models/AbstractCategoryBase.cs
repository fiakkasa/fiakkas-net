using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public abstract record AbstractCategoryBase : AbstractBaseData, ICategoryTitle
{
    public string Title { get; init; } = string.Empty;
}
