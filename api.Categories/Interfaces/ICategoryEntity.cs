using api.Categories.Enums;

namespace api.Categories.Interfaces;

public interface ICategoryEntity : ICategoryBase
{
    CategoryType Kind { get; init; }
    Uri? Href { get; init; }
}
