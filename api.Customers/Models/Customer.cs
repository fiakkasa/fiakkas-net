using api.Customers.Interfaces;

namespace api.Customers.Models;

[ExcludeFromCodeCoverage]
public record Customer : BaseData, ICustomer
{
    public string Title { get; init; } = string.Empty;
    public Uri? Href { get; init; }
}
