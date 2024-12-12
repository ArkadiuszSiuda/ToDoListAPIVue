using ToDoListAPI.Dtos;

namespace ToDoListAPI.Interfaces;

public interface IAssignmentsRepository
{
    Task<List<AssignmentDto>> GetAssignments();
    Task<List<AssignmentDto>> FilterAssignmentsByDate(DateTime dateTime);
    Task<AssignmentDto> GetAssignment(Guid id);
    Task CreateAssignment(AssignmentDto assignmentDto);
    Task UpdateAssignment(AssignmentDto assignmentDto);
    Task DeleteAssignment(Guid id);
}
