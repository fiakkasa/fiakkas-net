using api.Shared.Interfaces;
using api.Technologies.Extensions;
using api.Technologies.Interfaces;
using api.Technologies.Models;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace api.Technologies.Tests;

public class GraphFixture
{
    private IRequestExecutor? _requestExecutor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<IRequestExecutor> GetRequestExecutor()
    {
        if (_requestExecutor is not { })
        {
            _semaphore.Wait();

            var dataRepository = new MockDataRepository<ITechnology>(
            [
                new Technology
                {
                    Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative)
                }
            ]);

            _requestExecutor =
                await new ServiceCollection()
                    .AddSingleton<IDataRepository<ITechnology>>(dataRepository)
                    .AddGraphQL()
                    .AddQueryType()
                    .AddFiltering()
                    .AddSorting()
                    .AddApiTechnologies()
                    .BuildRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
