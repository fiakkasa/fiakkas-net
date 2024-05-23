using api.Customers.Extensions;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace api.Customers.Tests;

public class GraphFixture
{
    private IRequestExecutor? _requestExecutor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<IRequestExecutor> GetRequestExecutor()
    {
        if (_requestExecutor is not { })
        {
            _semaphore.Wait();

            var service = Substitute.For<IDataRepository<ICustomer>>();
            service
                .Get(Arg.Any<Func<ICustomer, Customer>>())
                .Returns(new[]
                {
                    new Customer
                    {
                        Id = Guid.Empty,
                        CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        UpdatedAt = null,
                        Version = 1,
                        Title = "Title",
                        Href = new Uri("/test", UriKind.Relative)
                    }
                }.AsQueryable());
            _requestExecutor =
                await new ServiceCollection()
                    .AddSingleton(service)
                    .AddGraphQL()
                    .AddQueryType()
                    .AddFiltering()
                    .AddSorting()
                    .AddApiCustomers()
                    .BuildRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
