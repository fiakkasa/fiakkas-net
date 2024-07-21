using api.Categories.Enums;

namespace api.Categories.Interfaces;

public interface ICategoryKind
{
    CategoryType Kind { get; init; }
}
