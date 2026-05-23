using TaskManager.Application.DTOs.Tasks;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetAllAsync(
        Guid userId);

    Task<TaskResponse?> GetByIdAsync(
        Guid id,
        Guid userId);

    Task CreateAsync(
        CreateTaskRequest request,
        Guid userId);

    Task UpdateAsync(
        Guid id,
        UpdateTaskRequest request,
        Guid userId);

    Task DeleteAsync(
        Guid id,
        Guid userId);
}