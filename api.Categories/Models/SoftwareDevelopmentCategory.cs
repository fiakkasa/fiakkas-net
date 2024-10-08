using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record SoftwareDevelopmentCategory :
    AbstractTechnologyCategory,
    IPolymorphicCategory,
    IPolymorphicTechnologyCategory;
