using System.Data;
using Microsoft.Data.SqlClient;

namespace TaskManager.Infrastructure.Repositories;

public abstract class BaseRepository
{
    protected readonly string _connectionString;

    protected BaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected SqlConnection CreateConnection()
        => new SqlConnection(_connectionString);
}