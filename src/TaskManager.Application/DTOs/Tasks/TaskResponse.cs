using TaskManager.Core.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public class TaskResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus  Status { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
}