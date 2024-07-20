using api.Customers.Interfaces;

namespace api.Customers.Models;

[ExcludeFromCodeCoverage]
public record CustomerEntity : AbstractBaseData, ICustomer
{
    public string Title { get; init; } = string.Empty;
    public Uri? Href { get; init; }
}
