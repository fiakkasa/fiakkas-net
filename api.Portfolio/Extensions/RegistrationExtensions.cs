using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Portfolio.Queries;
using api.Portfolio.Services;

namespace api.Portfolio.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiPortfolio(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services.AddBoundOptions<PortfolioDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<IPortfolioItem>, PortfolioItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiPortfolio(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension(typeof(PortfolioItemQueries));
}
