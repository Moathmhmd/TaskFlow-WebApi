using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();

    Task<ProjectDto?> GetByIdAsync(Guid id);

    Task CreateAsync(CreateProjectDto dto);
}