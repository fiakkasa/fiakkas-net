using api.Customers.Interfaces;
using api.Customers.Mappers;
using api.Customers.Models;

namespace api.Customers.DataLoaders;

[ExcludeFromCodeCoverage]
public sealed class CustomerBatchDataLoader(
    IDataRepository<ICustomer> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options
) : AbstractGenericBatchDataLoaderById<ICustomer, Customer>(
    dataRepository,
    CustomerMappers.Map,
    batchScheduler,
    options
);
