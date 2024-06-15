using api.Application.Models;
using api.Application.Queries;
using api.Application.TypeExtensions;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace api.Application.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiApplication(
        this IServiceCollection services,
        DateTimeOffset start,
        AssemblyInformationalVersionAttribute? versionInfo = default
    ) =>
        services.AddSingleton(
            new SystemInfoItem(
                versionInfo?.InformationalVersion ?? string.Empty,
                start
            )
        );

    public static IRequestExecutorBuilder AddApiApplication(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension<SystemQueries>()
            .AddTypeExtension<HealthQueries>()
            .AddTypeExtension<SystemInfoItemTypeExtension>();
}
