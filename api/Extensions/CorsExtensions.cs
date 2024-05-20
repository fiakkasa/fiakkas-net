namespace api.Extensions;

public static class CorsExtensions
{
    public static IApplicationBuilder UseApiCors(this IApplicationBuilder app) =>
        app.UseCors(options =>
            options
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .WithMethods(HttpMethods.Post)
        );
}
