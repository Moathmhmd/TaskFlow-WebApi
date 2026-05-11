using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllAsync(
        TaskStatuses? status,
        TaskPriority? priority);

    Task<TaskDto> GetByIdAsync(Guid id);

    Task<TaskDto> CreateAsync(CreateTaskDto dto);

    Task UpdateAsync(Guid id, UpdateTaskDto dto);

    Task UpdateStatusAsync(
        Guid id,
        UpdateTaskStatusDto dto);

    Task DeleteAsync(Guid id);
}