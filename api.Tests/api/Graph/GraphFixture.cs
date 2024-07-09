using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Application.Models;
using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Models;
using api.ContactItems.Interfaces;
using api.ContactItems.Models;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.Extensions;
using api.Languages.Enums;
using api.Languages.Interfaces;
using api.Languages.Models;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using api.TextItems.Interfaces;
using api.TextItems.Models;
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

            var achievementsDataRepository = new MockDataRepository<IAchievement>(
            [
                new Achievement
                {
                    Id = new Guid("d4605b0c-58bc-49ac-bcfd-10a24a203add"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Content = "Content",
                    Years = [2024]
                }
            ]);
            var categoryDataRepository = new MockDataRepository<ICategoryEntity>(
            [
                new CategoryEntity
                {
                    Kind = CategoryType.None,
                    Id = new Guid("c9f5879d-4018-49a0-9b71-b479dd5de7ff"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                },
                new CategoryEntity
                {
                    Kind = CategoryType.Portfolio,
                    Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                },
                new CategoryEntity
                {
                    Kind = CategoryType.Resume,
                    Id = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    AssociatedCategoryTypes = [CategoryType.SoftwareDevelopment]
                },
                new CategoryEntity
                {
                    Kind = CategoryType.SoftwareDevelopment,
                    Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative)
                },
                new CategoryEntity
                {
                    Kind = CategoryType.InformationTechnology,
                    Id = new Guid("6cc43e9a-312b-4923-b890-e966b8168eee"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative)
                },
                new CategoryEntity
                {
                    Kind = CategoryType.Other,
                    Id = new Guid("9fd91f8a-2a44-4520-a5b1-8aa1c3c29133"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title"
                }
            ]);
            var contactItemsDataRepository = new MockDataRepository<IContactItem>(
            [
                new ContactItem
                {
                    Id = new Guid("ebf224a8-7ff3-47b9-882b-dd41ec7f5a05"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Key = "Key",
                    Icon = "Icon",
                    Title = "Title",
                    Description = "Content",
                    Href = new Uri("/test", UriKind.Relative)
                }
            ]);
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
            var educationItemsDataRepository = new MockDataRepository<IEducationItem<EducationTimePeriod>>(
            [
                new EducationItem
                {
                    Id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    CategoryId = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                    TimePeriod = new()
                    {
                        Start = new DateOnly(2024, 1, 1),
                        End = null
                    },
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative),
                    Location = "Location",
                    Description = "Description",
                    Subjects = ["Subject"]
                }
            ]);
            var languageDataRepository = new MockDataRepository<ILanguage>(
            [
                new Language
                {
                    Id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Proficiency = ProficiencyType.Native
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
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative),
                    TechnologyIds = [new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")],
                    CustomerId = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109")
                }
            ]);
            var textItemDataRepository = new MockDataRepository<ITextItem>(
            [
                new TextItem
                {
                    Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
                    CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Key = "Key",
                    Title = "Title",
                    Content = "Content"
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
                    .AddSingleton<IDataRepository<IAchievement>>(achievementsDataRepository)
                    .AddSingleton<IDataRepository<ICategoryEntity>>(categoryDataRepository)
                    .AddSingleton<IDataRepository<IContactItem>>(contactItemsDataRepository)
                    .AddSingleton<IDataRepository<ICustomer>>(customerDataRepository)
                    .AddSingleton<IDataRepository<IEducationItem<EducationTimePeriod>>>(educationItemsDataRepository)
                    .AddSingleton<IDataRepository<ILanguage>>(languageDataRepository)
                    .AddSingleton<IDataRepository<IPortfolioItem>>(portfolioItemDataRepository)
                    .AddSingleton<IDataRepository<ITextItem>>(textItemDataRepository)
                    .AddSingleton(new SystemInfoItem("Version", new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)))
                    .AddSingleton(healthCheckService)
                    .AddApiGraphQL(isDev: false)
                    .BuildServiceProvider()
                    .GetRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
