using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Validators;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(
        ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskResponse>>
        GetAllAsync(Guid userId)
    {
        var tasks =
            await _repository.GetByUserAsync(userId);

        return tasks.Select(t => new TaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            StatusName = t.Status.ToString(),
            DueDate = t.DueDate
        });
    }

    public async Task CreateAsync(
        CreateTaskRequest request,
        Guid userId)
    {
        TaskValidator.Validate(request);

        var entity = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            UserId = userId
        };

        await _repository.CreateAsync(entity);
    }

    public async Task<TaskResponse?>
        GetByIdAsync(Guid id, Guid userId)
    {
        var task =
            await _repository.GetByIdAsync(
                id,
                userId);

        if (task is null)
            return null;

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            StatusName = task.Status.ToString(),
            DueDate = task.DueDate
        };
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateTaskRequest request,
        Guid userId)
    {
        var existing =
            await _repository.GetByIdAsync(
                id,
                userId);

        if (existing is null)
            throw new Exception("Task not found");

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Status = request.Status;
        existing.DueDate = request.DueDate;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(
        Guid id,
        Guid userId)
    {
        await _repository.DeleteAsync(
            id,
            userId);
    }
}