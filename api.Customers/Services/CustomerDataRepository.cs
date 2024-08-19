using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Services;

public sealed class CustomerDataRepository(
    ILogger<CustomerDataRepository> logger,
    IOptionsSnapshot<CustomersDataConfig> dataSnapshot
) : AbstractReadOnlyInMemoryDataRepository<ICustomer, CustomersDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<ICustomer>? ResolveSet(CustomersDataConfig data) => data.Customers;
}
