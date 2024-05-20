using api.Customers.Interfaces;

namespace api.Customers.Models;

[ExcludeFromCodeCoverage]
public record Customer : BaseData, ICustomer
{
    public required string Title { get; init; }
    public Uri? Href { get; init; }
}
