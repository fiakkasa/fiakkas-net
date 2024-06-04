namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record CategoriesDataConfig
{
    public CategoryEntity[] Categories { get; init; } = [];
}
