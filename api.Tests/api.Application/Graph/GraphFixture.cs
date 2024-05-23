using api.Application.Extensions;
using api.Application.Models;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Application.Tests;

public class GraphFixture
{
    private IRequestExecutor? _requestExecutor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<IRequestExecutor> GetRequestExecutor()
    {
        if (_requestExecutor is not { })
        {
            _semaphore.Wait();

            var service = Substitute.For<HealthCheckService>();
            service
                .CheckHealthAsync(Arg.Any<CancellationToken>())
                .Returns(
                    new HealthReport(
                        entries: new Dictionary<string, HealthReportEntry>(),
                        status: HealthStatus.Healthy,
                        totalDuration: TimeSpan.FromSeconds(10)
                    )
                );
            _requestExecutor =
                await new ServiceCollection()
                    .AddSingleton(new SystemInfoItem("Version", new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)))
                    .AddSingleton(service)
                    .AddGraphQL()
                    .AddQueryType()
                    .AddApiApplication()
                    .BuildRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
