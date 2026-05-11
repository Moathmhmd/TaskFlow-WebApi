using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;
public class UpdateTaskStatusDto
{
    public TaskStatuses Status { get; set; }
}
