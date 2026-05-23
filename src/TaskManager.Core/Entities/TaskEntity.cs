using TaskManager.Core.Enums ;

namespace TaskManager.Core.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus  Status { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid UserId { get; set; }
}