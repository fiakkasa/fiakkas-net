namespace api.Customers.Models;

[ExcludeFromCodeCoverage]
public record CustomersDataConfig
{
    public CustomerEntity[] Customers { get; init; } = [];
}
