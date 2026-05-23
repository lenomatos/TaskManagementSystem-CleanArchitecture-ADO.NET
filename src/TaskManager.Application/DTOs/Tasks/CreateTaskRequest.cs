using TaskManager.Core.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus  Status { get; set; }

    public DateTime? DueDate { get; set; }
}