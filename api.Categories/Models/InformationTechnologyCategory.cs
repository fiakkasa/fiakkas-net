using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record InformationTechnologyCategory : AbstractCategoryBase, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
