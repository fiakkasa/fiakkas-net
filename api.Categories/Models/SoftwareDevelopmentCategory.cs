using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record SoftwareDevelopmentCategory : AbstractCategoryBase, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
