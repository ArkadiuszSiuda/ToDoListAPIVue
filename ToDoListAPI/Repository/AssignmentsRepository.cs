using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Db;
using ToDoListAPI.Dtos;
using ToDoListAPI.Interfaces;

namespace ToDoListAPI.Repository;

public class AssignmentsRepository : IAssignmentsRepository
{
    private readonly ToDoListContext _context;

    public AssignmentsRepository(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<List<AssignmentDto>> GetAssignments()
    {
        var dbAssignments = await _context.Assignments.ToListAsync();
        var dtoAssignements = dbAssignments.Select(AssignmentDto.MapToAssignmentDto).ToList();

        return dtoAssignements;
    }

    public async Task<AssignmentDto> GetAssignment(Guid id)
    {
        var dbAssignments = await _context.Assignments.ToListAsync();
        var dtoAssignement = dbAssignments.Select(AssignmentDto.MapToAssignmentDto).FirstOrDefault(t => t.Id == id) ?? throw new Exception($"Assignment with id {id} not found");

        return dtoAssignement;
    }

    public async Task<List<AssignmentDto>> FilterAssignmentsByDate(DateTime dateTime)
    {
        var dbAssignments = await _context.Assignments.ToListAsync();
        var dtoAssignements = dbAssignments.Where(t => t.Deadline.Year == dateTime.Year && t.Deadline.Month == dateTime.Month && t.Deadline.Day == dateTime.Day).Select(AssignmentDto.MapToAssignmentDto).ToList();

        return dtoAssignements;
    }

    public async Task CreateAssignment(AssignmentDto assignmentDto)
    {
        var mappedAssignment = AssignmentDto.MapFromAssignmentDto(assignmentDto);
        await _context.AddAsync(mappedAssignment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAssignment(AssignmentDto assignmentDto)
    {
        var dbAssignment = await _context.Assignments.FirstOrDefaultAsync(t => t.Id == assignmentDto.Id) ?? throw new Exception($"Assignment with id {assignmentDto.Id} not found");
        AssignmentDto.MapFromAssignmentDto(assignmentDto, dbAssignment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAssignment(Guid id)
    {
        var assignment = await _context.Assignments.FirstOrDefaultAsync(t => t.Id == id) ?? throw new Exception($"Assignment with id {id} not found");
        _context.Remove(assignment);
        await _context.SaveChangesAsync();
    }
}
