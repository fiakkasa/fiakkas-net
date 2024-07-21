using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public abstract record AbstractTechnologyCategory : AbstractCategoryBase, ITechnologyCategory
{
    public Uri? Href { get; init; }
}
