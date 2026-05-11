using AutoMapper;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IGenericRepository<Project> _repository;

    private readonly ICurrentUserService _currentUserService;
    private readonly IGenericRepository<TaskItem> _taskRepository;
    private readonly IMapper _mapper;

    public ProjectService(
        IGenericRepository<Project> repository,
        ICurrentUserService currentUserService,
        IGenericRepository<TaskItem> taskRepository,
        IMapper mapper)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var userId = _currentUserService.GetUserId();

        var projects = await _repository.FindAsync(
            x => x.UserId == userId);

        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto> GetByIdAsync(Guid id)
    {
        var userId = _currentUserService.GetUserId();

        var project = await _repository.GetByIdAsync(id);

        if (project is null)
            throw new NotFoundException("Project not found");

        if (project.UserId != userId)
            throw new UnauthorizedException(
                "You cannot access this project");

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(
        CreateProjectDto dto)
    {
        ValidateProject(dto.Name);

        var project = _mapper.Map<Project>(dto);

        project.UserId = _currentUserService.GetUserId();

        await _repository.AddAsync(project);

        await _repository.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateProjectDto dto)
    {
        ValidateProject(dto.Name);

        var userId = _currentUserService.GetUserId();

        var project = await _repository.GetByIdAsync(id);

        if (project is null)
            throw new NotFoundException("Project not found");

        if (project.UserId != userId)
            throw new UnauthorizedException(
                "You cannot update this project");

        project.Name = dto.Name;

        _repository.Update(project);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var userId = _currentUserService.GetUserId();

        var project = await _repository.GetByIdAsync(id);

        if (project is null)
            throw new NotFoundException("Project not found");

        if (project.UserId != userId)
            throw new UnauthorizedException(
                "You cannot delete this project");

        _repository.Delete(project);

        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskDto>>
    GetProjectTasksAsync(Guid projectId)
    {
        var userId = _currentUserService.GetUserId();

        var project =
            await _repository.GetByIdAsync(projectId);

        if (project is null)
            throw new NotFoundException(
                "Project not found");

        if (project.UserId != userId)
            throw new UnauthorizedException(
                "You cannot access this project");

        var tasks =
            await _taskRepository.FindAsync(
                x => x.ProjectId == projectId);

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }
    private static void ValidateProject(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException(
                "Project name is required");

        if (name.Length > 100)
            throw new BadRequestException(
                "Project name cannot exceed 100 characters");
    }
}