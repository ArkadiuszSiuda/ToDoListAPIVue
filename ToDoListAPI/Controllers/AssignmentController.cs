using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Dtos;
using ToDoListAPI.Entities;
using ToDoListAPI.Interfaces;

namespace ToDoListAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentsRepository _assignmentsRepository;

    public AssignmentController(IAssignmentsRepository assignmentsRepository)
    {
        _assignmentsRepository = assignmentsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<AssignmentDto>>> GetAllAssignemtns()
    {
        return Ok(await _assignmentsRepository.GetAssignments());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssignmentDto>> GetAssignment(Guid id)
    {
        return Ok(await _assignmentsRepository.GetAssignment(id));
    }

    [HttpGet("filter-by-date/{dateTime}")]
    public async Task<ActionResult<List<AssignmentDto>>> FilterAssignmentsByDate(DateTime dateTime)
    {
        return Ok(await _assignmentsRepository.FilterAssignmentsByDate(dateTime));
    }

    [HttpPost]
    public async Task<ActionResult<AssignmentDto>> CreateAssignment(AssignmentDto assignmentDto)
    {
        await _assignmentsRepository.CreateAssignment(assignmentDto);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<Assignment>> UpdateAssignment(AssignmentDto assignmentDto)
    {
        await _assignmentsRepository.UpdateAssignment(assignmentDto);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Assignment>> DeleteAssignment(Guid id)
    {
        await _assignmentsRepository.DeleteAssignment(id);
        return NoContent();
    }
}
