namespace api.GraphExtensions.Extensions;

public static class RegistrationExtensions
{
    public static IRequestExecutorBuilder AddApiGraphExtensions(this IRequestExecutorBuilder builder) =>
        builder.AddGraphExtensionsGraph();
}
