using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class TaskDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public TaskPriority Priority { get; set; }

    public TaskStatuses Status { get; set; }

    public Guid ProjectId { get; set; }
}