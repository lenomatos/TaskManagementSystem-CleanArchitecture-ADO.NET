using System.Data;
using Microsoft.Data.SqlClient;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username",
            conn);

        cmd.Parameters.AddWithValue("@Username", username);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new UserEntity
        {
            Id = reader.GetGuid(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2)
        };
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Id = @Id",
            conn);

        cmd.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new UserEntity
        {
            Id = reader.GetGuid(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2)
        };
    }

    public async Task CreateAsync(UserEntity user)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "INSERT INTO Users (Id, Username, PasswordHash) VALUES (@Id, @Username, @PasswordHash)",
            conn);

        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}