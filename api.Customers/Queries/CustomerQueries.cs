using api.Customers.DataLoaders;
using api.Customers.Interfaces;
using api.Customers.Mappers;
using api.Customers.Models;

namespace api.Customers.Queries;

[QueryType]
public static class CustomerQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Customer> GetCustomers([Service] IDataRepository<ICustomer> repository) =>
        repository.Get(CustomerMappers.Map);

    [NodeResolver]
    public static async ValueTask<Customer?> GetCustomerById(
        Guid id,
        CustomerBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
