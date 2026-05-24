using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// optional: load .env only in development
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddScoped<IUserRepository>(_ =>
    new UserRepository(connectionString!));

builder.Services.AddScoped<ITaskRepository>(_ =>
    new TaskRepository(connectionString!));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
