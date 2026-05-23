using TaskManager.Application.DTOs.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Application.Validators;

public static class TaskValidator
{
    public static void Validate(CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title required");

        if (request.Title.Length > 100)
            throw new ArgumentException(
                "Title max length is 100");

        if (!Enum.IsDefined(
            typeof(TaskItemStatus ),
            request.Status))
        {
            throw new ArgumentException(
                "Invalid status");
        }

        if (request.DueDate.HasValue &&
            request.DueDate.Value.Date < DateTime.UtcNow.Date)
        {
            throw new ArgumentException(
                "DueDate cannot be in the past");
        }
    }
}