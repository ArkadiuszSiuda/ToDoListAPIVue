using ToDoListAPI.Entities;

namespace ToDoListAPI.Dtos;

public class AssignmentDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Added { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; } = false;

    public static AssignmentDto MapToAssignmentDto(Assignment assignment)
    {
        return new AssignmentDto
        {
            Id = assignment.Id,
            Name = assignment.Name,
            Description = assignment.Description,
            Added = assignment.Added,
            Deadline = assignment.Deadline,
            IsCompleted = assignment.IsCompleted
        };
    }
    public static Assignment MapFromAssignmentDto(AssignmentDto assignmentDto, Assignment? assignment = null)
    {
        if (assignment is null) {
            assignment = new Assignment { Id = Guid.NewGuid(), Added = DateTime.Now };
        }

        assignment.Name = assignmentDto.Name;
        assignment.Description = assignmentDto.Description;
        assignment.Deadline = assignmentDto.Deadline.ToLocalTime();
        assignment.IsCompleted = assignmentDto.IsCompleted;

        return assignment;
    }
}
