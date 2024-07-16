using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public abstract record CategoryBase : BaseData, ICategory
{
    public string Title { get; init; } = string.Empty;
}
