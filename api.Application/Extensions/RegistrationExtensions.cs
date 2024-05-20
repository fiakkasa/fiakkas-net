using api.Application.Models;
using api.Application.Queries;
using api.Application.TypeExtensions;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace api.Application.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiApplication(this IServiceCollection services, Assembly assembly) =>
        services.AddSingleton(
            new SystemInfoItem(
                assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty,
                DateTimeOffset.Now
            )
        );

    public static IRequestExecutorBuilder AddApiApplication(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension<SystemQueries>()
            .AddTypeExtension<HealthQueries>()
            .AddTypeExtension<SystemInfoItemTypeExtension>();
}
