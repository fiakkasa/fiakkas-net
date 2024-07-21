using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public record InformationTechnologyCategory :
    AbstractTechnologyCategory,
    IPolymorphicCategory,
    IPolymorphicTechnologyCategory
{ }
