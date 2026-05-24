using Microsoft.Data.SqlClient;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Enums;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<TaskEntity>> GetByUserAsync(Guid userId)
    {
        var result = new List<TaskEntity>();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "SELECT Id, Title, Description, Status, DueDate, UserId FROM Tasks WHERE UserId = @UserId",
            conn);

        cmd.Parameters.AddWithValue("@UserId", userId);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(Map(reader));
        }

        return result;
    }

    public async Task<TaskEntity?> GetByIdAsync(Guid taskId, Guid userId)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "SELECT Id, Title, Description, Status, DueDate, UserId FROM Tasks WHERE Id = @Id AND UserId = @UserId",
            conn);

        cmd.Parameters.AddWithValue("@Id", taskId);
        cmd.Parameters.AddWithValue("@UserId", userId);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return Map(reader);
    }

    public async Task CreateAsync(TaskEntity task)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            @"INSERT INTO Tasks (Id, Title, Description, Status, DueDate, UserId)
              VALUES (@Id, @Title, @Description, @Status, @DueDate, @UserId)",
            conn);

        cmd.Parameters.AddWithValue("@Id", task.Id);
        cmd.Parameters.AddWithValue("@Title", task.Title);
        cmd.Parameters.AddWithValue("@Description", (object?)task.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", (int)task.Status);
        cmd.Parameters.AddWithValue("@DueDate", (object?)task.DueDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@UserId", task.UserId);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(TaskEntity task)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            @"UPDATE Tasks
              SET Title = @Title,
                  Description = @Description,
                  Status = @Status,
                  DueDate = @DueDate
              WHERE Id = @Id AND UserId = @UserId",
            conn);

        cmd.Parameters.AddWithValue("@Id", task.Id);
        cmd.Parameters.AddWithValue("@UserId", task.UserId);
        cmd.Parameters.AddWithValue("@Title", task.Title);
        cmd.Parameters.AddWithValue("@Description", (object?)task.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", (int)task.Status);
        cmd.Parameters.AddWithValue("@DueDate", (object?)task.DueDate ?? DBNull.Value);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid taskId, Guid userId)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "DELETE FROM Tasks WHERE Id = @Id AND UserId = @UserId",
            conn);

        cmd.Parameters.AddWithValue("@Id", taskId);
        cmd.Parameters.AddWithValue("@UserId", userId);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private TaskEntity Map(SqlDataReader reader)
    {
        return new TaskEntity
        {
            Id = reader.GetGuid(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            Status = (TaskItemStatus)reader.GetInt32(3),
            DueDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
            UserId = reader.GetGuid(5)
        };
    }
}