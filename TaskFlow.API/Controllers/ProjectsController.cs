using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _service.GetAllAsync();

        return Ok(new ApiResponse<IEnumerable<ProjectDto>>(
        true,
        "Projects retrieved successfully",
        projects));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        await _service.CreateAsync(dto);

        return Ok(new ApiResponse<string>(
         true,
         "Project created successfully",
         null));
    }
}