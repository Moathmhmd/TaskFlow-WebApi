using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;
public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public TaskPriority Priority { get; set; }

    public Enums.TaskStatus Status { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;
}
