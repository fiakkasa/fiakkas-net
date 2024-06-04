using api.Application.Models;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.Extensions;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using api.Technologies.Interfaces;
using api.Technologies.Models;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Tests;

public class GraphFixture
{
    private IRequestExecutor? _requestExecutor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<IRequestExecutor> GetRequestExecutor()
    {
        if (_requestExecutor is not { })
        {
            _semaphore.Wait();

            var customerDataRepository = new MockDataRepository<ICustomer>(
            [
                new Customer
                {
                    Id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative)
                }
            ]);
            var portfolioItemDataRepository = new MockDataRepository<IPortfolioItem>([
                new PortfolioItem
                {
                    Id = new Guid("28e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Year = 2024,
                    CategoryId = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                    Ordinal = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative),
                    TechnologyIds = [new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109")],
                    CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
                }
            ]);
            var categoryDataRepository = new MockDataRepository<ICategory>(
            [
                new Category
                {
                    Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                }
            ]);
            var technologyDataRepository = new MockDataRepository<ITechnology>(
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
            var healthCheckService = Substitute.For<HealthCheckService>();
            healthCheckService
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
                    .AddSingleton<IDataRepository<ICustomer>>(customerDataRepository)
                    .AddSingleton<IDataRepository<IPortfolioItem>>(portfolioItemDataRepository)
                    .AddSingleton<IDataRepository<ICategory>>(categoryDataRepository)
                    .AddSingleton<IDataRepository<ITechnology>>(technologyDataRepository)
                    .AddSingleton(new SystemInfoItem("Version", new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)))
                    .AddSingleton(healthCheckService)
                    .AddApiGraphQLServer(isDev: false)
                    .AddApiGraphQLEndpoints()
                    .BuildRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
