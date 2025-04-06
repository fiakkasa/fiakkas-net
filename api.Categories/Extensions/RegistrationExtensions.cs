using api.Categories.Interfaces;
using api.Categories.Models;
using api.Categories.Services;

namespace api.Categories.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiCategories(
        this IServiceCollection services,
        string sectionPath = "data"
    )
    {
        services.AddValidatedOptions<CategoriesDataConfig>(sectionPath);

        services.AddScoped<IDataRepository<ICategory>, CategoryDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiCategories(this IRequestExecutorBuilder builder) =>
        builder
            .AddCategoriesGraph()
            .AddInterfaceType<IPolymorphicCategory>(descriptor => descriptor.Field(f => f.Id).ID())
            .AddInterfaceType<IPolymorphicTechnologyCategory>(descriptor => descriptor.Field(f => f.Id).ID())
            .AddObjectType<UnknownTechnologyCategory>(descriptor =>
            {
                descriptor.Field(f => f.Id).ID();
                descriptor
                    .ImplementsNode()
                    .ResolveNode(_ => ValueTask.FromResult<object?>(default));
            });
}
