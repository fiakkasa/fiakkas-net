using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record OtherCategory : AbstractCategoryBase, IPolymorphicCategory { }
