using AutoMapper;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services;

public class TaskService : ITaskService
{
    private readonly IGenericRepository<TaskItem> _taskRepository;

    private readonly IGenericRepository<Project> _projectRepository;

    private readonly ICurrentUserService _currentUserService;

    private readonly IMapper _mapper;

    public TaskService(
        IGenericRepository<TaskItem> taskRepository,
        IGenericRepository<Project> projectRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDto>> GetAllAsync(
        TaskStatuses? status,
        TaskPriority? priority)
    {
        var userId = _currentUserService.GetUserId();

        var projects = await _projectRepository.FindAsync(
            x => x.UserId == userId);

        var projectIds = projects.Select(x => x.Id);

        var tasks = await _taskRepository.FindAsync(
            x => projectIds.Contains(x.ProjectId));

        if (status.HasValue)
        {
            tasks = tasks.Where(
                x => x.Status == status.Value);
        }

        if (priority.HasValue)
        {
            tasks = tasks.Where(
                x => x.Priority == priority.Value);
        }

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto> GetByIdAsync(Guid id)
    {
        var userId = _currentUserService.GetUserId();

        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new NotFoundException("Task not found");

        var project =
            await _projectRepository.GetByIdAsync(
                task.ProjectId);

        if (project!.UserId != userId)
            throw new UnauthorizedException(
                "You cannot access this task");

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(
        CreateTaskDto dto)
    {
        ValidateTask(
            dto.Title,
            dto.Description,
            dto.DueDate);

        var userId = _currentUserService.GetUserId();

        var project =
            await _projectRepository.GetByIdAsync(
                dto.ProjectId);

        if (project is null)
            throw new NotFoundException(
                "Project not found");

        if (project.UserId != userId)
            throw new UnauthorizedException(
                "You cannot add tasks to this project");

        var task = _mapper.Map<TaskItem>(dto);

        task.Status = TaskStatuses.Todo;

        await _taskRepository.AddAsync(task);

        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateTaskDto dto)
    {
        ValidateTask(
            dto.Title,
            dto.Description,
            dto.DueDate);

        var userId = _currentUserService.GetUserId();

        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new NotFoundException("Task not found");

        var project =
            await _projectRepository.GetByIdAsync(
                task.ProjectId);

        if (project!.UserId != userId)
            throw new UnauthorizedException(
                "You cannot update this task");

        if (task.Status == TaskStatuses.Completed)
            throw new BadRequestException(
                "Completed tasks cannot be updated");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.Priority = dto.Priority;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(
        Guid id,
        UpdateTaskStatusDto dto)
    {
        var userId = _currentUserService.GetUserId();

        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new NotFoundException("Task not found");

        var project =
            await _projectRepository.GetByIdAsync(
                task.ProjectId);

        if (project!.UserId != userId)
            throw new UnauthorizedException(
                "You cannot update this task");

        task.Status = dto.Status;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var userId = _currentUserService.GetUserId();

        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new NotFoundException("Task not found");

        var project =
            await _projectRepository.GetByIdAsync(
                task.ProjectId);

        if (project!.UserId != userId)
            throw new UnauthorizedException(
                "You cannot delete this task");

        _taskRepository.Delete(task);

        await _taskRepository.SaveChangesAsync();
    }

    private static void ValidateTask(
        string title,
        string description,
        DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new BadRequestException(
                "Task title is required");

        if (title.Length > 100)
            throw new BadRequestException(
                "Task title cannot exceed 100 characters");

        if (description.Length > 500)
            throw new BadRequestException(
                "Description cannot exceed 500 characters");

        if (dueDate < DateTime.UtcNow)
            throw new BadRequestException(
                "Due date cannot be in the past");
    }
}