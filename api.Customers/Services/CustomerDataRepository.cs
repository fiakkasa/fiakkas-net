using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Services;

public class CustomerDataRepository(ILogger<CustomerDataRepository> logger, IOptionsSnapshot<CustomersDataConfig> dataSnapshot)
: AbstractDataRepository<ICustomer, CustomersDataConfig>(logger, dataSnapshot)
{
    protected override T[]? ResolveSet<T>(CustomersDataConfig data) => data.Customers switch
    {
        T[] result => result,
        _ => null,
    };
}
