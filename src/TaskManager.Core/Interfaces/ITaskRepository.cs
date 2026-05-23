using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(
        Guid id,
        Guid userId);

    Task<IEnumerable<TaskEntity>> GetByUserAsync(
        Guid userId);

    Task CreateAsync(
        TaskEntity task);

    Task UpdateAsync(
        TaskEntity task);

    Task DeleteAsync(
        Guid id,
        Guid userId);
}