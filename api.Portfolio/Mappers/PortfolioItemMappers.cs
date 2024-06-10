using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Mappers;

public static class PortfolioItemMappers
{
    public static PortfolioItem Map(this IPortfolioItem x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Year = x.Year,
            CategoryId = x.CategoryId,
            Title = x.Title,
            Href = x.Href,
            TechnologyIds = x.TechnologyIds,
            CustomerId = x.CustomerId
        };
}
