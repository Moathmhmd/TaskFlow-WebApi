using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TaskStatuses? status, [FromQuery] TaskPriority? priority)
    {
        var tasks =
            await _service.GetAllAsync(
                status,
                priority);

        return Ok(new ApiResponse<IEnumerable<TaskDto>>(
            true,
            "Tasks retrieved successfully",
            tasks));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _service.GetByIdAsync(id);

        return Ok(new ApiResponse<TaskDto>(
            true,
            "Task retrieved successfully",
            task));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var createdTask =
            await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTask.Id },
            new ApiResponse<TaskDto>(
                true,
                "Task created successfully",
                createdTask));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpPatch("status/{id:guid}")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateTaskStatusDto dto)
    {
        await _service.UpdateStatusAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}