using api.Customers.Interfaces;
using api.Customers.Models;
using api.Customers.Services;

namespace api.Customers.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiCustomers(
        this IServiceCollection services,
        string sectionPath = "data"
    )
    {
        services.AddValidatedOptions<CustomersDataConfig>(sectionPath);

        services.AddScoped<IDataRepository<ICustomer>, CustomerDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiCustomers(this IRequestExecutorBuilder builder) =>
        builder.AddCustomersGraph();
}
