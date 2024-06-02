using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Mappers;

public static class PortfolioCategoryMappers
{
    public static PortfolioCategory Map(this IPortfolioCategory x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title
        };
}
