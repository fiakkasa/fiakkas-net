using api.ContactItems.Interfaces;
using api.ContactItems.Models;
using api.ContactItems.Services;

namespace api.ContactItems.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiContactItems(
        this IServiceCollection services,
        IConfiguration config,
        string sectionPath = "data"
    )
    {
        services.AddBoundOptions<ContactItemsDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<IContactItem>, ContactItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiContactItems(this IRequestExecutorBuilder builder) =>
        builder.AddContactItemsGraph();
}
