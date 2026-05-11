using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();

    Task<ProjectDto> GetByIdAsync(Guid id);

    Task<ProjectDto> CreateAsync(CreateProjectDto dto);

    Task UpdateAsync(Guid id, UpdateProjectDto dto);

    Task DeleteAsync(Guid id);
    Task<IEnumerable<TaskDto>> GetProjectTasksAsync(Guid projectId);
}