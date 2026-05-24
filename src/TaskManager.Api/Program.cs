
using TaskManager.Api.Extensions;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureEnvironment();

var connectionString =
    builder.Configuration.GetRequiredConnectionString();

builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository>(_ =>
    new UserRepository(connectionString));

builder.Services.AddScoped<ITaskRepository>(_ =>
    new TaskRepository(connectionString));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.ConfigureJwtAuthentication(
    builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();