using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Validators;
using TaskManager.Core.Enums;

namespace TaskManager.Tests.Validators;

public class TaskValidatorTests
{
    [Fact]
    public void Validate_ShouldThrow_WhenTitleIsEmpty()
    {
        var request = new CreateTaskRequest
        {
            Title = "",
            Status = TaskItemStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            TaskValidator.Validate(request));

        Assert.Contains("Title", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenTitleIsLongerThan100Characters()
    {
        var request = new CreateTaskRequest
        {
            Title = new string('A', 101),
            Status = TaskItemStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            TaskValidator.Validate(request));

        Assert.Contains("100", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenStatusIsInvalid()
    {
        var request = new CreateTaskRequest
        {
            Title = "Valid title",
            Status = (TaskItemStatus)999,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            TaskValidator.Validate(request));

        Assert.Contains("status", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenDueDateIsInPast()
    {
        var request = new CreateTaskRequest
        {
            Title = "Valid title",
            Status = TaskItemStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            TaskValidator.Validate(request));

        Assert.Contains("past", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenRequestIsValid()
    {
        var request = new CreateTaskRequest
        {
            Title = "Valid title",
            Description = "Valid description",
            Status = TaskItemStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var exception = Record.Exception(() =>
            TaskValidator.Validate(request));

        Assert.Null(exception);
    }
}