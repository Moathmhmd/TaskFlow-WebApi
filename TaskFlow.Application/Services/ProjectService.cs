using AutoMapper;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IGenericRepository<Project> _repository;

    private readonly IMapper _mapper;

    public ProjectService(
        IGenericRepository<Project> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);

        return project is null
            ? null
            : _mapper.Map<ProjectDto>(project);
    }

    public async Task CreateAsync(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        await _repository.AddAsync(project);

        await _repository.SaveChangesAsync();
    }
}