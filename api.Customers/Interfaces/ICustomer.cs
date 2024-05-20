namespace api.Customers.Interfaces;

public interface ICustomer : IBaseData
{
    string Title { get; init; }
    Uri? Href { get; init; }
}
