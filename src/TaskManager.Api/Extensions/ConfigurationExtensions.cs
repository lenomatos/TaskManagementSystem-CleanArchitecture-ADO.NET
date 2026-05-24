using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Api.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureEnvironment(
        this WebApplicationBuilder builder)
    {
        var rootPath =
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..",
                    "..",
                    "..",
                    ".."));

        var envPath =
            Path.Combine(rootPath, ".env");

        if (!File.Exists(envPath))
        {
            throw new FileNotFoundException(
                $".env file not found at: {envPath}");
        }

        DotNetEnv.Env.Load(envPath);

        builder.Configuration.AddEnvironmentVariables();
    }

    public static void ConfigureJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSecret =
            configuration["JWT_SECRET"]
            ?? throw new InvalidOperationException("JWT_SECRET not found.");

        var jwtIssuer =
            configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer not found.");

        var jwtAudience =
            configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience not found.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSecret))
                    };
            });

        services.AddAuthorization();
    }

    public static string GetRequiredConnectionString(
        this IConfiguration configuration)
    {
        return configuration["CONNECTION_STRING"]
            ?? throw new InvalidOperationException(
                "CONNECTION_STRING not found.");
    }
}