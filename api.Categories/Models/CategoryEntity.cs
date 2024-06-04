using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record CategoryEntity : BaseData, ICategory
{
    public string Title { get; init; } = string.Empty;
}
