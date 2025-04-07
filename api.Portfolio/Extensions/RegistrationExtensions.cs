using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Portfolio.Queries;
using api.Portfolio.Services;

namespace api.Portfolio.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiPortfolio(
        this IServiceCollection services,
        string sectionPath = "data"
    )
    {
        services.AddValidatedOptions<PortfolioDataConfig>(sectionPath);

        services.AddScoped<IDataRepository<IPortfolioItem>, PortfolioItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiPortfolio(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<PortfolioItemBatchDataLoader>()
            .AddTypeExtension(typeof(PortfolioItemQueries));
}
