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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _service.GetByIdAsync(id);

        return Ok(new ApiResponse<ProjectDto>(
            true,
            "Project retrieved successfully",
            project));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateProjectDto dto)
    {
        var createdProject =
            await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProject.Id },
            new ApiResponse<ProjectDto>(
                true,
                "Project created successfully",
                createdProject));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProjectDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("tasks/{id:guid}")]
    public async Task<IActionResult> GetProjectTasks(
    Guid id)
    {
        var tasks =
            await _service.GetProjectTasksAsync(id);

        return Ok(new ApiResponse<IEnumerable<TaskDto>>(
            true,
            "Project tasks retrieved successfully",
            tasks));
    }
}