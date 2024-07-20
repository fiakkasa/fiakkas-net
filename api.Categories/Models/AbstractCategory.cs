using api.Categories.Interfaces;

namespace api.Categories.Models;

[ExcludeFromCodeCoverage]
public abstract record AbstractCategory : AbstractCategoryBase, ICategory { }
