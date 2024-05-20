using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Mappers;

public static class CustomerMappers
{
    public static Customer Map(this ICustomer x) =>
        new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Version = x.Version,
            Title = x.Title,
            Href = x.Href
        };
}
