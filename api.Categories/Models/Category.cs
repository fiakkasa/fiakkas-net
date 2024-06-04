using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record Category : BaseData, ICategory
{
    public string Title { get; init; } = string.Empty;
}
