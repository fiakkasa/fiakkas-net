using api.Customers.Interfaces;
using api.Customers.Mappers;
using api.Customers.Models;

namespace api.Customers.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class CustomerQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Customer> GetCustomers([Service] IDataRepository<ICustomer> repository) =>
        repository.Get(CustomerMappers.Map);
}
