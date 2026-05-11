using AutoMapper;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();

        CreateMap<CreateProjectDto, Project>().ReverseMap();

        CreateMap<TaskItem, TaskDto>().ReverseMap();

        CreateMap<CreateTaskDto, TaskItem>().ReverseMap();
    }
}