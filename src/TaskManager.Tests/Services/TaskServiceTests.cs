using Moq;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;

namespace TaskManager.Tests.Services;

public class TaskServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyUserTasks()
    {
        var userId = Guid.NewGuid();

        var repository = new Mock<ITaskRepository>();

        repository
            .Setup(x => x.GetByUserAsync(userId))
            .ReturnsAsync(new List<TaskEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    Status = TaskItemStatus.Pending,
                    UserId = userId
                }
            });

        var service = new TaskService(repository.Object);

        var result = await service.GetAllAsync(userId);

        Assert.Single(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenTitleIsInvalid()
    {
        var repository = new Mock<ITaskRepository>();
        var service = new TaskService(repository.Object);

        var request = new CreateTaskRequest
        {
            Title = "",
            Status = TaskItemStatus.Pending
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepository_WhenValid()
    {
        var repository = new Mock<ITaskRepository>();
        var service = new TaskService(repository.Object);

        var request = new CreateTaskRequest
        {
            Title = "Valid task",
            Description = "Test",
            Status = TaskItemStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        await service.CreateAsync(request, Guid.NewGuid());

        repository.Verify(
            x => x.CreateAsync(It.IsAny<TaskEntity>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenTaskDoesNotExist()
    {
        var repository = new Mock<ITaskRepository>();

        repository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((TaskEntity?)null);

        var service = new TaskService(repository.Object);

        var request = new UpdateTaskRequest
        {
            Title = "Updated",
            Status = TaskItemStatus.InProgress,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.UpdateAsync(Guid.NewGuid(), request, Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository_WhenTaskExists()
    {
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var repository = new Mock<ITaskRepository>();

        repository
            .Setup(x => x.GetByIdAsync(taskId, userId))
            .ReturnsAsync(new TaskEntity
            {
                Id = taskId,
                UserId = userId,
                Title = "Task",
                Status = TaskItemStatus.Pending
            });

        var service = new TaskService(repository.Object);

        await service.DeleteAsync(taskId, userId);

        repository.Verify(
            x => x.DeleteAsync(taskId, userId),
            Times.Once);
    }
}