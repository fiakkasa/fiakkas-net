using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Services;

public sealed class CustomerDataRepository(
    ILogger<CustomerDataRepository> logger,
    IOptionsSnapshot<CustomersDataConfig> dataSnapshot
)
    : AbstractInMemoryDataRepository<ICustomer, CustomersDataConfig>(logger, dataSnapshot)
{
    protected override ICustomer[]? ResolveSet(CustomersDataConfig data) => data.Customers;
}
