using api.Customers.Interfaces;
using api.Customers.Mappers;
using api.Customers.Models;

namespace api.Customers.Queries;

[QueryType]
public static class CustomerQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Customer> GetCustomers([Service] IDataRepository<ICustomer> repository) =>
        repository.Get(CustomerMappers.Map);
}
