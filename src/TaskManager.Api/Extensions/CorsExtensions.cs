namespace TaskManager.Api.Extensions;

public static class CorsExtensions
{
    public const string WebUiPolicy = "WebUiPolicy";

    public static void ConfigureCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigin =
            configuration["CORS_ALLOWED_ORIGIN"]
            ?? throw new InvalidOperationException(
                "CORS_ALLOWED_ORIGIN is missing.");

        services.AddCors(options =>
        {
            options.AddPolicy(WebUiPolicy, policy =>
            {
                policy
                    .WithOrigins(allowedOrigin)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}